import React, { useState } from 'react';
import { NavMenu } from '../Nav/NavMenu';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import AlarmsTable, { FilterAlarmTime, FilterAlarmPriority } from './AlarmsTable';
import TagsTable, { FilterTagTime, FilterTagId, FilterInputTag } from './TagsTable';

export default function Reports() {
    const [selectedTable, setSelectedTable] = useState('Table A');
    const [tableData, setTableData] = useState([]);

    const handleFilter = data => {
        setTableData(data); // Update filtered data based on the filter API response
    };

    const renderTable = () => {
        if (selectedTable === 'Table A') {
            return <div><FilterAlarmTime onFilter={handleFilter} />
                        <AlarmsTable data={tableData} />
                   </div>;
        } else if (selectedTable === 'Table B') {
            return <div><FilterAlarmPriority onFilter={handleFilter} />
                        <AlarmsTable data={tableData} />
                   </div>;
        } else if (selectedTable === 'Table C') {
            return <div><FilterTagTime onFilter={handleFilter} />
                <TagsTable data={tableData} />
            </div>;
        } else if (selectedTable === 'Table D') {
            const filterProps = {
                onFilter: handleFilter,
                type: 'AI',
            };
            return <div><FilterInputTag filterProps={filterProps} />
                <TagsTable data={tableData} />
            </div>;
        }
        else if (selectedTable === 'Table E') {
            const filterProps = {
                onFilter: handleFilter,
                type: 'DI',
            };
            return <div><FilterInputTag filterProps={filterProps} />
                <TagsTable data={tableData} />
            </div>;
        }
        else if (selectedTable === 'Table F') {
            return <div><FilterTagId onFilter={handleFilter} />
                <TagsTable data={tableData} />
            </div>;
        }
    };

    return (
        <div>
            <NavMenu showNavbar={true}></NavMenu>
            <Dropdown onSelect={setSelectedTable} />
            {renderTable()}
        </div>
    );
}

function Dropdown({ onSelect }) {
    const options = ['Table A', 'Table B', 'Table C', 'Table D', 'Table E', 'Table F'];
    const [selectedOption, setSelectedOption] = useState(options[0]);

    const handleSelect = (event) => {
        setSelectedOption(event.target.value);
        onSelect(event.target.value);
    };

    return (
        <div>
            <select value={selectedOption} onChange={handleSelect}>
                {options.map((option, index) => (
                    <option key={index} value={option}>
                        {option}
                    </option>
                ))}
            </select>
        </div>
    );
}

