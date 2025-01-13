import React, { useState, useEffect } from 'react';
import {
    CRow,
    CCol,
    CCard,
    CCardHeader,
    CCardBody,
    CButtonGroup,
    CButton,
    CFormSelect,
} from '@coreui/react';
import axios from 'axios';
import { CChartBar, CChartLine, CChartPie } from '@coreui/react-chartjs';
import LineChartComparison from '../LineChartComparison ';
import ReportSection from '../report/ReportSection';

const ElectricityConsumption = () => {
    // Generate past 10 years for dropdown
    const currentYear = new Date().getFullYear();
    const [years, setYears] = useState(Array.from({ length: 10 }, (_, i) => currentYear - i));

    const [electricBuildings, setElectricBuildings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [selectedYear, setSelectedYear] = useState(currentYear);
    const [selectedBuilding, setSelectedBuilding] = useState([]);
    const [barChartViewMode, setBarChartViewMode] = useState('Month'); // Bar Chart
    const [barChartSelecredBuilding, setBarChartSelectedBuilding] = useState([]);
    const [barChartSelectedYear, setBarChartSelectedYear] = useState(currentYear);


    const [lineChartViewMode, setLineChartViewMode] = useState('Month'); // Line Chart
    const [pieChartViewMode, setPieChartViewMode] = useState('Month'); // Pie Chart

    // Store random data to prevent re-generation on re-renders
    const generateRandomData = (length) => Array.from({ length }, () => Math.floor(Math.random() * 500) + 50);
    const [monthlyData, setMonthlyData] = useState(generateRandomData(12));
    const [yearlyData, setYearlyData] = useState(generateRandomData(10));
    const [totalMonthlyData, setTotalMonthlyData] = useState([]);
    const [totalYearlyData, setTotalYearlyData] = useState([]);
    // Mock department-wise consumption
    const departmentData = {
        HR: Math.floor(Math.random() * 1000),
        IT: Math.floor(Math.random() * 1500),
        Operations: Math.floor(Math.random() * 2000),
        Finance: Math.floor(Math.random() * 1200),
    };


    const fetchBuildings = async () => {
        try {
            const response = await axios.get('http://localhost:5154/api/Building');

            // Filter buildings with non-null e_MeterCode
            const filteredBuildings = response.data.buildings
                .filter(building => building.e_MeterCode !== null)
                .map(building => ({
                    id: building.id,
                    label: building.name,
                    value: building.e_MeterCode
                }));

            setElectricBuildings(filteredBuildings);

            // Set the default selected building to the first item
            if (filteredBuildings.length > 0) {
                setBarChartSelectedBuilding(filteredBuildings[2]);
            }

            setLoading(false);
        } catch (error) {
            console.error('Error fetching buildings:', error);
            setError('Failed to load building data.');
            setLoading(false);
        }
    };

    const fetchElectricDataYearly = async () => {
        const buildingId = barChartSelecredBuilding.id;
        const baseUrl = "http://localhost:5154/api/Electric";
        const url = `${baseUrl}?buildingId=${buildingId}`;

        try {
            const response = await axios.get(url);

            if (response.status === 200 && response.data && response.data.electrics) {
                const electrics = response.data.electrics;

                // Process data for monthly and yearly charts
                const months = Array(12).fill(0); // Initialize array for 12 months
                const yearlyConsumption = {};

                electrics.forEach((item) => {
                    const date = new Date(item.date); // Assuming API returns a `date` field
                    const year = date.getFullYear();
                    const month = date.getMonth(); // 0-based index for months (Jan = 0)

                    // Aggregate monthly consumption
                    months[month] += item.usage;

                    // Aggregate yearly consumption
                    yearlyConsumption[year] = (yearlyConsumption[year] || 0) + item.usage;
                });

                //setMonthlyData(months);
                setYearlyData(Object.values(yearlyConsumption));
                setYears(Object.keys(yearlyConsumption));
            } else {
                console.error("Invalid response format or data not found.");
            }
        } catch (error) {
            console.error("Error fetching electric data:", error.message);
        }
    };

    // Fetch electricity data based on selected building and year
    const fetchElectricData = async () => {
        try {
            const startDate = `${barChartSelectedYear}-01-01T00:00:00Z`;
            const endDate = `${barChartSelectedYear}-12-31T23:59:59Z`;

            const response = await axios.get(`http://localhost:5154/api/Electric`, {
                params: {
                    buildingId: barChartSelecredBuilding.id,
                    startDate: startDate,
                    endDate: endDate
                }
            });

            // Handle the data (e.g., set to state)
            console.log(response.data.electrics);
            const electricityData = response.data.electrics;

            // Process the monthly data (assuming response contains monthly breakdown)
            const monthlyConsumption = Array(12).fill(0); // Initialize an array for 12 months
            electricityData.forEach(entry => {
                const month = new Date(entry.date).getMonth(); // Get month (0 = Jan, 11 = Dec)
                monthlyConsumption[month] += entry.usage; // Accumulate consumption for the month
                console.log('Month:', month + 1, 'Usage:', entry.usage);

            });

            setMonthlyData(monthlyConsumption); // Update state with monthly consumption

        } catch (error) {
            console.error('Error fetching electricity data:', error);
            setError('Failed to load electricity data.');
        }
    };

    // Fetch total yearly data (all buildings)
    const fetchTotalElectricDataYearly = async () => {
        try {
            const response = await axios.get("http://localhost:5154/api/Electric");

            if (response.status === 200 && response.data && response.data.electrics) {
                const gasUsage = response.data.electrics;

                const yearlyConsumption = {};
                gasUsage.forEach((item) => {
                    const year = new Date(item.date).getFullYear();
                    yearlyConsumption[year] = (yearlyConsumption[year] || 0) + item.usage;
                });

                setTotalYearlyData(Object.values(yearlyConsumption));
                setYears(Object.keys(yearlyConsumption));
            } else {
                console.error("Invalid response format or data not found.");
            }
        } catch (error) {
            console.error("Error fetching total yearly natural gas data:", error.message);
        }
    };

    // Fetch total monthly data (all buildings for selected year)
    const fetchTotalElectricData = async () => {
        try {
            const startDate = `${barChartSelectedYear}-01-01T00:00:00Z`;
            const endDate = `${barChartSelectedYear}-12-31T23:59:59Z`;

            const response = await axios.get("http://localhost:5154/api/Electric", {
                params: { startDate, endDate },
            });

            if (response.status === 200 && response.data && response.data.electrics) {
                const gasUsage = response.data.electrics;

                const monthlyConsumption = Array(12).fill(0);
                gasUsage.forEach((item) => {
                    const month = new Date(item.date).getMonth();
                    monthlyConsumption[month] += item.usage;
                });

                setTotalMonthlyData(monthlyConsumption);
            } else {
                console.error("Invalid response format or data not found.");
            }
        } catch (error) {
            console.error("Error fetching total monthly electric data:", error.message);
        }
    };

    useEffect(() => {
        fetchBuildings();
        fetchElectricData();
        fetchElectricDataYearly();
    }, []);

    useEffect(() => {
        if (barChartSelecredBuilding) {
            fetchElectricData();
        }
    }, [barChartSelecredBuilding, barChartSelectedYear,]);

    useEffect(() => {
        fetchTotalElectricDataYearly();
        fetchTotalElectricData();
    }, [barChartSelectedYear]);

    useEffect(() => {
        if (barChartSelecredBuilding) {
            fetchElectricDataYearly();
        }
    }, [barChartSelecredBuilding]);

    const handleBuildingChange = (e) => {
        const selectedValue = e.target.value;
        const barChartSelecredBuilding = electricBuildings.find(building => building.value === selectedValue);
        setBarChartSelectedBuilding(barChartSelecredBuilding);
    };


    // Loading state
    if (loading) {
        return <div>Loading buildings...</div>;
    }

    // Error state
    if (error) {
        return <div className="text-danger">{error}</div>;
    }

    return (
        <div>
            {/* Global Year and Building Selection */}
            <CRow className="mb-3 d-flex justify-content-between">
                <CCol xs="auto">
                    <h3>Electricity Consumption</h3>
                </CCol>
            </CRow>

            <ReportSection type="electric" />

            {/* Bar Chart Card */}
            <CRow>
                <CCol>
                    <CCard className="mb-4">
                        <CCardHeader>
                            <CRow>
                                <CCol>
                                    <h6>{barChartViewMode}ly Consumption</h6>
                                </CCol>
                                <CCol xs="auto">
                                    <CFormSelect
                                        className="form-control-sm me-2"
                                        value={barChartSelecredBuilding?.value || ""}
                                        onChange={handleBuildingChange}
                                    >
                                        <option value="">All Buildings</option>
                                        {electricBuildings.map((building) => (
                                            <option key={building.value} value={building.value}>
                                                {building.label}
                                            </option>
                                        ))}
                                    </CFormSelect>
                                </CCol>
                                <CCol xs="auto">
                                    {barChartViewMode === "Month" && years.length > 0 && (
                                        <CFormSelect
                                            className="form-control-sm"
                                            value={barChartSelectedYear}
                                            onChange={(e) => setBarChartSelectedYear(parseInt(e.target.value))}
                                        >
                                            {years.map((year) => (
                                                <option key={year} value={year}>
                                                    {year}
                                                </option>
                                            ))}
                                        </CFormSelect>
                                    )}
                                </CCol>
                                <CCol xs="auto">
                                    <CButtonGroup>
                                        {["Month", "Year"].map((value) => (
                                            <CButton
                                                color="outline-secondary"
                                                key={value}
                                                active={value === barChartViewMode}
                                                onClick={() => setBarChartViewMode(value)}
                                            >
                                                {value}
                                            </CButton>
                                        ))}
                                    </CButtonGroup>
                                </CCol>
                            </CRow>
                        </CCardHeader>
                        <CCardBody className="d-flex flex-column" style={{ minHeight: "400px" }}>
                            <CChartBar
                                data={{
                                    labels:
                                        barChartViewMode === "Month"
                                            ? ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
                                            : years,
                                    datasets: [
                                        {
                                            label: barChartSelecredBuilding
                                                ? `${barChartSelecredBuilding.label} Consumption`
                                                : "Total Consumption",
                                            backgroundColor: "#f87979",
                                            data: barChartSelecredBuilding
                                                ? barChartViewMode === "Month"
                                                    ? monthlyData
                                                    : yearlyData
                                                : barChartViewMode === "Month"
                                                    ? totalMonthlyData
                                                    : totalYearlyData,
                                        },
                                    ],
                                }}
                                options={{
                                    responsive: true,
                                    maintainAspectRatio: false,
                                }}
                                style={{ flexGrow: 1 }}
                            />
                        </CCardBody>
                    </CCard>
                </CCol>
            </CRow>

            <CRow className='mb-3'>
                {/* Line Chart Comparison */}
                <LineChartComparison
                    buildings={electricBuildings}
                    type="electric"
                />
            </CRow>




        </div>
    );
};

export default ElectricityConsumption;


