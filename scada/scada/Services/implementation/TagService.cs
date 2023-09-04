﻿using scada.Data;
using scada.Models;
using scada.Services.interfaces;
using scada.Exceptions;
using Newtonsoft.Json;
using scada.DTO;
using scada.Drivers;
using scada.Data.Config;

namespace scada.Services.implementation
{
    public class TagService : ITagService
    {
        private List<Tag> _tags;
        private ITagHistoryService _tagHistoryService = new TagHistoryService();

        public TagService() 
        {
            _tags = Load();    
        }

        private List<Tag> Load()
        {
            return XmlSerializationHelper.LoadFromXml<Tag>();
        }

        public List<Tag> Get()
        {
            return _tags;
        }

        public Tag? Get(int id)
        {
            foreach (Tag tag in _tags) if (tag.Id == id)  return tag; 
            throw new NotFoundException("Tag not found!");
        }

        public List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();

            foreach (Tag tag in _tags) { 
                if (tag is AITag)
                {
                    AITag aitag = (AITag) tag;
                    alarms.AddRange(aitag.Alarms);
                }
            }

            return alarms;
        }

        public Alarm GetAlarmById(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag is AITag)
                {
                    AITag aitag = (AITag)tag;
                    Alarm alarm = aitag.Alarms.FirstOrDefault(item => item.Id == id);
                    if (alarm != null) return alarm;
                }
            }
            throw new NotFoundException("Alarm not found!");
        }

        public AITag GetTagByAlarmId(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag is AITag)
                {
                    AITag aitag = (AITag)tag;
                    Alarm alarm = aitag.Alarms.FirstOrDefault(item => item.Id == id);
                    if (alarm != null) return aitag;
                }
            }
            throw new NotFoundException("Tag not found!");
        }

        public List<DOTag> GetDOTags() 
        {
            return ConfigHelper.ParseTags<DOTag>(_tags); 
        }

        public List<AOTag> GetAOTags()
        {
            return ConfigHelper.ParseTags<AOTag>(_tags);
        }

        public List<DITag> GetDITags()
        {
            return ConfigHelper.ParseTags<DITag>(_tags);
        }

        public List<AITag> GetAITags()
        {
            return ConfigHelper.ParseTags<AITag>(_tags);
        }

        public bool Delete(int id)
        {
            foreach (Tag tag in _tags)
            {
                if (tag.Id == id) { 
                    _tags.Remove(tag);
                    _tagHistoryService.Delete(id);
                    XmlSerializationHelper.SaveToXml(_tags);
                    return true; 
                }
            }
            throw new NotFoundException("Tag not found!");
        }

        public Tag Insert(TagDTO tagDTO)
        {
            List<String> addresses = getAllAddresses();
            Tag tag = convert(tagDTO);

            if (tag != null)
            {
                if (tagDTO.Type == "AOTag" || tagDTO.Type == "DOTag")
                {
                    if (addresses.Contains(tag.Address))
                        throw new BadRequestException("Address already in use!");
                }
                tag.Id = generateId();
                _tags.Add(tag);
                XmlSerializationHelper.SaveToXml(_tags);
                return tag;
            }

            throw new BadRequestException("Invalid tag data"); ;
        }

        private Tag convert(TagDTO tagDTO)
        {
            return tagDTO.Type switch
            {
                "DOTag" => JsonConvert.DeserializeObject<DOTag>(tagDTO.Data.ToString()),
                "DITag" => JsonConvert.DeserializeObject<DITag>(tagDTO.Data.ToString()),
                "AOTag" => JsonConvert.DeserializeObject<AOTag>(tagDTO.Data.ToString()),
                "AITag" => JsonConvert.DeserializeObject<AITag>(tagDTO.Data.ToString()),
                _ => null // handle unknown types
            };
        }

        private int generateId()
        {
            int id = 1;
            foreach (Tag tag in _tags) if (tag.Id > id) id = tag.Id;
            return ++id;
        }

        private List<String> getAllAddresses()
        {
            List<String> addresses = new List<String>();
            addresses.AddRange(new[] { "a1", "a2", "a3", "a4", "a5",
                                       "d1", "d2", "d3", "d4", "d5"});
            return addresses;
        }

        public void ReceiveRTUValue(RTUData rtu)
        {
            RTUDriver.SetValue(rtu.Address, rtu.Value);
        }
    }
}
