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
    CFormCheck,
    CSpinner,
    CAlert,

} from '@coreui/react';
import axios from 'axios';
import { CChartBar } from '@coreui/react-chartjs';
import LineChartComparison from '../LineChartComparison ';
import ReportSection from '../report/ReportSection';

const NaturalGasConsumption = () => {
    const currentYear = new Date().getFullYear();
    const [years, setYears] = useState(Array.from({ length: 10 }, (_, i) => currentYear - i));

    const [gasBuildings, setGasBuildings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [barChartSelectedBuilding, setBarChartSelectedBuilding] = useState(null);
    const [barChartSelectedYear, setBarChartSelectedYear] = useState(currentYear);
    const [barChartViewMode, setBarChartViewMode] = useState('Year');

    const generateRandomData = (length) => Array.from({ length }, () => Math.floor(Math.random() * 500) + 50);
    const [monthlyData, setMonthlyData] = useState(generateRandomData(12));
    const [yearlyData, setYearlyData] = useState(generateRandomData(10));
    const [totalMonthlyData, setTotalMonthlyData] = useState([]);
    const [totalYearlyData, setTotalYearlyData] = useState([]);
    const [includeGraphs, setIncludeGraphs] = useState(true); // State for graph inclusion
    const [downloadingReport, setDownloadingReport] = useState(false); // State to track downloading





    const fetchBuildings = async () => {
        try {
            const response = await axios.get('http://localhost:5154/api/Building');

            const filteredBuildings = response.data.buildings
                .filter(building => building.g_MeterCode !== null)
                .map(building => ({
                    id: building.id,
                    label: building.name,
                    value: building.g_MeterCode,
                }));

            setGasBuildings(filteredBuildings);

            if (filteredBuildings.length > 0) {
                setBarChartSelectedBuilding(filteredBuildings[0]);
            }

            setLoading(false);
        } catch (error) {
            console.error('Error fetching buildings:', error);
            setError('Failed to load building data.');
            setLoading(false);
        }
    };

    const fetchNaturalGasDataYearly = async () => {
        if (!barChartSelectedBuilding) return;

        const buildingId = barChartSelectedBuilding.id;
        const baseUrl = "http://localhost:5154/api/NaturalGas";
        const url = `${baseUrl}?buildingId=${buildingId}`;

        try {
            const response = await axios.get(url);

            if (response.status === 200 && response.data && response.data.naturalGas) {
                const gasUsage = response.data.naturalGas;

                const months = Array(12).fill(0);
                const yearlyConsumption = {};

                gasUsage.forEach((item) => {
                    const date = new Date(item.date);
                    const year = date.getFullYear();
                    const month = date.getMonth();

                    months[month] += item.usage;
                    yearlyConsumption[year] = (yearlyConsumption[year] || 0) + item.usage;
                });

                setYearlyData(Object.values(yearlyConsumption));
                setYears(Object.keys(yearlyConsumption));
            } else {
                console.error("Invalid response format or data not found.");
            }
        } catch (error) {
            console.error("Error fetching natural gas data:", error.message);
        }
    };

    const fetchNaturalGasData = async () => {
        if (!barChartSelectedBuilding) return;

        try {
            const startDate = `${barChartSelectedYear}-01-01T00:00:00Z`;
            const endDate = `${barChartSelectedYear}-12-31T23:59:59Z`;

            const response = await axios.get(`http://localhost:5154/api/NaturalGas`, {
                params: {
                    buildingId: barChartSelectedBuilding.id,
                    startDate: startDate,
                    endDate: endDate,
                },
            });

            const gasData = response.data.naturalGas;

            const monthlyConsumption = Array(12).fill(0);
            gasData.forEach((entry) => {
                const month = new Date(entry.date).getMonth();
                monthlyConsumption[month] += entry.usage;
            });

            setMonthlyData(monthlyConsumption);
        } catch (error) {
            console.error('Error fetching natural gas data:', error);
            setError('Failed to load natural gas data.');
        }
    };

    // Fetch total yearly data (all buildings)
    const fetchTotalNaturalGasDataYearly = async () => {
        try {
            const response = await axios.get("http://localhost:5154/api/NaturalGas");

            if (response.status === 200 && response.data && response.data.naturalGas) {
                const gasUsage = response.data.naturalGas;

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
    const fetchTotalNaturalGasData = async () => {
        try {
            const startDate = `${barChartSelectedYear}-01-01T00:00:00Z`;
            const endDate = `${barChartSelectedYear}-12-31T23:59:59Z`;

            const response = await axios.get("http://localhost:5154/api/NaturalGas", {
                params: { startDate, endDate },
            });

            if (response.status === 200 && response.data && response.data.naturalGas) {
                const gasUsage = response.data.naturalGas;

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
            console.error("Error fetching total monthly natural gas data:", error.message);
        }
    };


    useEffect(() => {
        fetchBuildings();
        fetchNaturalGasData();
        fetchNaturalGasDataYearly();
    }, []);

    useEffect(() => {
        if (barChartSelectedBuilding) {
            fetchNaturalGasData();
        }
    }, [barChartSelectedBuilding, barChartSelectedYear]);

    useEffect(() => {
        if (barChartSelectedBuilding) {
            fetchNaturalGasDataYearly();
        }
    }, [barChartSelectedBuilding]);

    useEffect(() => {
        fetchTotalNaturalGasDataYearly();
        fetchTotalNaturalGasData();
    }, [barChartSelectedYear]);

    const handleBuildingChange = (e) => {
        const selectedValue = e.target.value;
        const selectedBuilding = gasBuildings.find(building => building.value === selectedValue);
        setBarChartSelectedBuilding(selectedBuilding);
    };

    if (loading) {
        return <div>Loading buildings...</div>;
    }

    if (error) {
        return <div className="text-danger">{error}</div>;
    }

    return (
        <div>
            <CRow className="mb-3 d-flex justify-content-between">
                <CCol xs="auto">
                    <h3>Natural Gas Consumption</h3>
                </CCol>
            </CRow>


            <ReportSection type="naturalGas" />

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
                                        value={barChartSelectedBuilding?.value || ""}
                                        onChange={handleBuildingChange}
                                    >
                                        <option value="">All Buildings</option>
                                        {gasBuildings.map((building) => (
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
                                            label: barChartSelectedBuilding
                                                ? `${barChartSelectedBuilding.label} Consumption`
                                                : "Total Consumption",
                                            backgroundColor: "#f87979",
                                            data: barChartSelectedBuilding
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

            <CRow className="mb-3">
                <LineChartComparison
                    buildings={gasBuildings}
                    type="naturalGas"
                />
            </CRow>
        </div>
    );
};

export default NaturalGasConsumption;
