﻿using Azure;
using scada.Database;
using scada.DTO;
using scada.Exceptions;
using scada.Models;
using scada.Services.implementation;

namespace scada.Services
{
    public class TagHistoryService : ITagHistoryService
    {

        public List<TagHistory> Get()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> tagHistory = dbContext.TagHistory.ToList();
                return tagHistory;
            }
        }

        public List<TagHistoryDTO> GetByTagId(int id)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                Tag tag = new TagService().Get(id);
                List<TagHistory> tagHistory = new List<TagHistory>();
                foreach (TagHistory th in dbContext.TagHistory.ToList()) if (th.TagId == id) tagHistory.Add(th);

                List<TagHistoryDTO> dto = new List<TagHistoryDTO>();
                foreach (TagHistory th in tagHistory) dto.Add(new TagHistoryDTO(tag, th));
                return dto;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetLastValueOfAITags()
        {
            List<AITag> tags = new TagService().GetAITags();

            using (var dbContext = new ApplicationDbContext())
            {
                var result = tags.GroupJoin(
                        dbContext.TagHistory.ToList(),
                        aitag => aitag.Id,
                        tagHistory => tagHistory.TagId,
                        (aitag, tagHistoryGroup) => new
                        {
                            AITag = aitag,
                            LastTagHistory = tagHistoryGroup
                                .OrderBy(th => th.Timestamp)
                                .LastOrDefault() // Get the last TagHistory with the least Timestamp value
                        })
                    .Select(result => new TagHistoryDTO(result.AITag, result.LastTagHistory))
                    .ToList();
                return result;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetLastValueOfDITags()
        {
            List<DITag> tags = new TagService().GetDITags();

            using (var dbContext = new ApplicationDbContext())
            {
                var result = tags.GroupJoin(
                        dbContext.TagHistory.ToList(),
                        ditag => ditag.Id,
                        tagHistory => tagHistory.TagId,
                        (ditag, tagHistoryGroup) => new
                        {
                            DITag = ditag,
                            LastTagHistory = tagHistoryGroup
                                .OrderBy(th => th.Timestamp)
                                .LastOrDefault() // Get the last TagHistory with the least Timestamp value
                        })
                    .Select(result => new TagHistoryDTO(result.DITag, result.LastTagHistory))
                    .ToList();
                return result;
            }
        }

        List<TagHistoryDTO> ITagHistoryService.GetTagsByTime(FilterDTO filter)
        {
            List<TagHistoryDTO> dto = new List<TagHistoryDTO>();

            using (var dbContext = new ApplicationDbContext())
            {
                List<TagHistory> filteredTagHistories = dbContext.TagHistory.ToList()
                .Where(th => th.Timestamp >= filter.StartDate && th.Timestamp <= filter.EndDate)
                .ToList();

                foreach (TagHistory th in filteredTagHistories)
                {
                    dto.Add(new TagHistoryDTO(new TagService().Get(th.TagId), th));
                }
            }

            return dto;
        }
    }
}
