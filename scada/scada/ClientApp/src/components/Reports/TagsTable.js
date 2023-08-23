import React, { useState, useEffect } from 'react';
import './Reports.css';
import '../../fonts.css';
import axios from 'axios';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export function FilterTagTime({ onFilter }) {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);

    const handleFilterClick = async () => {
        const filterOptions = {
            startDate,
            endDate,
        };

        try {
            //1. real data:
            /* const response = await axios.post('/api/filter', filterOptions); // Modify the API endpoint as needed
             onFilter(response.data); // Pass the filtered data to the parent component*/
            //2. test data:
            const response = await fetch('weatherforecast');
            const data = await response.json();
            onFilter(data);
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, []); // Empty dependency array means it runs only on mount

    return (
        <div>
            <div>
                <label>Start date: </label>
                <DatePicker selected={startDate} onChange={date => setStartDate(date)} />
            </div>
            <div>
                <label>End date: </label>
                <DatePicker selected={endDate} onChange={date => setEndDate(date)} />
            </div>
            <button onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export function FilterTagId({ onFilter }) {
    const [identificator, setIdentificator] = useState('');

    const handleFilterClick = async () => {
        try {
            //1. real data
            /*const response = await axios.post('/api/filter', { identificator });
            onFilter(response.data); // Pass the filtered data to the parent component*/
            //2. test data:
            const response = await fetch('weatherforecast');
            const data = await response.json();
            onFilter(data);
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, []); // Empty dependency array means it runs only on mount

    return (
        <div>
            <div>
                <label>Tag id: </label>
                <input
                    type="text"
                    value={identificator}
                    onChange={e => setIdentificator(e.target.value)}
                />
            </div>
            <button onClick={handleFilterClick}>Apply Filter</button>
        </div>
    );
}

export function FilterInputTag({ filterProps }) {
    const { onFilter, type } = filterProps;

    const handleFilterClick = async () => {
        try {
            //1. real data
            /*if (type === "AI") {
                const response = await axios.post('/api/filter');
                onFilter(response.data); // Pass the filtered data to the parent component
            }
            else if (type === "DI") {
                const response = await axios.post('/api/filter');
                onFilter(response.data); // Pass the filtered data to the parent component
            }*/
            //2. test data:
            const response = await fetch('weatherforecast');
            const data = await response.json();
            onFilter(data);
        } catch (error) {
            console.error('Error fetching filtered data:', error);
        }
    };

    useEffect(() => {
        handleFilterClick(); // Call the function on page load
    }, [type]); // it runs also whenever type is changed 

    return (
        <div>
        </div>
    );
}

export default function TagsTable({ data }) {
    return (
        <table>
            <thead>
                <tr>
                    <th>Column A</th>
                    <th>Column B</th>
                    <th>Column C</th>
                </tr>
            </thead>
            <tbody>
                {data.map((item, index) => (
                    <tr key={index}>
                        <td>{item.date}</td>
                        <td>{item.temperatureF}</td>
                        <td>{item.temperatureC}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}