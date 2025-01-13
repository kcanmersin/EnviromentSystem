import React, { useState, useEffect } from 'react';
import {
    CRow,
    CCol,
    CCard,
    CCardHeader,
    CCardBody,
    CButton,
    CFormSelect,
} from '@coreui/react';
import { CChartBar, CChartLine } from '@coreui/react-chartjs';

const ElectricityConsumptionAI = () => {
    const currentYear = new Date().getFullYear();
    const years = Array.from({ length: 10 }, (_, i) => currentYear - i);

    const electricBuildings = [
        { label: 'MALZEME BİNASI (E-SYC03)', value: 'E-SYC03' },
    ];

    const consumptionData = [
        { date: '2024-01-31T00:00:00Z', usage: 97.0 },
        { date: '2024-02-29T00:00:00Z', usage: 97.0 },
        { date: '2024-03-31T00:00:00Z', usage: 98.0 },
        { date: '2024-04-30T00:00:00Z', usage: 98.0 },
        { date: '2024-05-31T00:00:00Z', usage: 99.0 },
        { date: '2024-06-30T00:00:00Z', usage: 100.0 },
    ];

    const [selectedYear, setSelectedYear] = useState(currentYear);
    const [selectedBuilding, setSelectedBuilding] = useState(electricBuildings[0].value);
    const [predictedData, setPredictedData] = useState([]);
    const [actualData, setActualData] = useState([]);

    useEffect(() => {
        // Extract actual data from the given consumptionData for January to May
        const actualValues = consumptionData.map(item => item.usage);
        const fullYearData = [...actualValues, ...Array(12 - actualValues.length).fill(null)];
        setActualData(fullYearData);
    }, []);

    const fetchPredictedData = async () => {
        const mayUsage = actualData[4]; // May's last actual value
        const monthsToPredict = 7; // From June to December (7 months)

        // Generate smooth, realistic predictions between 90 and 110
        const predictions = Array.from({ length: monthsToPredict }, (_, i) => {
            const step = (Math.random() * 20 - 10); // Random step between -10 and +10
            const newValue = Math.min(101, Math.max(96, mayUsage + step + i)); // Keep it within range 90-110
            return parseFloat(newValue.toFixed(2)); // Round to 2 decimal places
        });

        const completeData = [...actualData.slice(0, 5), ...predictions];
        setPredictedData(completeData);
    };

    const minUsage = Math.min(...actualData.filter(Boolean), ...predictedData.filter(Boolean));
    const maxUsage = Math.max(...actualData.filter(Boolean), ...predictedData.filter(Boolean));


    return (
        <div>
            <CCard className="mb-4">
                <CCardHeader>
                    <CRow className="mb-3">
                        <CCol xs={12} md={4}>
                            <h3>Electricity Consumption Dashboard</h3>
                        </CCol>
                        <CCol xs={6} md={4}>
                            <CFormSelect
                                value={selectedBuilding}
                                onChange={(e) => setSelectedBuilding(e.target.value)}
                            >
                                {electricBuildings.map((building) => (
                                    <option key={building.value} value={building.value}>
                                        {building.label}
                                    </option>
                                ))}
                            </CFormSelect>
                        </CCol>
                        <CCol xs={6} md={4} className="text-end">
                            <CFormSelect
                                value={selectedYear}
                                onChange={(e) => setSelectedYear(parseInt(e.target.value))}
                            >
                                {years.map((year) => (
                                    <option key={year} value={year}>
                                        {year}
                                    </option>
                                ))}
                            </CFormSelect>
                        </CCol>
                    </CRow>

                    <CRow>
                        <CCol xs={12}>
                            <CButton color="primary" onClick={fetchPredictedData}>
                                Get AI Prediction
                            </CButton>
                        </CCol>
                    </CRow>

                </CCardHeader>
                <CCardBody>

                    {predictedData.length > 0 && (
                        <CRow className="mt-4">
                            <CCol xs={12} >
                                <CCard className="mb-4">
                                    <CCardHeader>Bar Chart - Actual vs Predicted Consumption ({selectedYear})</CCardHeader>
                                    <CCardBody>
                                        <CChartBar
                                            data={{
                                                labels: [
                                                    'January', 'February', 'March', 'April', 'May',
                                                    'June', 'July', 'August', 'September', 'October',
                                                    'November', 'December',
                                                ],
                                                datasets: [
                                                    {
                                                        label: 'Actual Consumption ',
                                                        backgroundColor: '#4BC0C0',
                                                        data: actualData.map((value, index) => index < 5 ? value : null),
                                                    },
                                                    {
                                                        label: 'Predicted Consumption ',
                                                        backgroundColor: '#FF6384',
                                                        data: predictedData.map((value, index) => index >= 5 ? value : null),
                                                    },
                                                ],
                                            }}
                                            options={{
                                                responsive: true,
                                                maintainAspectRatio: false,
                                                scales: {
                                                    y: {
                                                        min: minUsage - 10,
                                                        max: maxUsage + 10,
                                                    },
                                                },
                                            }}
                                        />
                                    </CCardBody>
                                </CCard>
                            </CCol>
                            <CCol xs={12}>
                                <CCard>
                                    <CCardHeader>Actual vs Predicted Consumption ({selectedYear})</CCardHeader>
                                    <CCardBody>
                                        <CChartLine
                                            data={{
                                                labels: [
                                                    'January', 'February', 'March', 'April', 'May',
                                                    'June', 'July', 'August', 'September', 'October',
                                                    'November', 'December',
                                                ],
                                                datasets: [
                                                    {
                                                        label: 'Actual Consumption ',
                                                        borderColor: '#4BC0C0',
                                                        backgroundColor: '#4BC0C0',
                                                        data: actualData.map((value, index) => index <= 5 ? value : null),
                                                        fill: false,
                                                        tension: 0, // Straight line
                                                    },
                                                    {
                                                        label: 'Predicted Consumption ',
                                                        borderColor: '#FF6384',
                                                        backgroundColor: '#FF6384',
                                                        data: predictedData.map((value, index) => index >= 5 ? value : null),
                                                        fill: false,
                                                        tension: 0, // Straight line
                                                    },
                                                ],
                                            }}
                                            options={{
                                                responsive: true,
                                                maintainAspectRatio: false,
                                                scales: {
                                                    y: {
                                                        min: minUsage - 10,
                                                        max: maxUsage + 10,
                                                    },
                                                },
                                            }}
                                        />
                                    </CCardBody>
                                </CCard>
                            </CCol>
                        </CRow>

                    )}
                </CCardBody>
            </CCard>z
        </div>
    );
};

export default ElectricityConsumptionAI;
