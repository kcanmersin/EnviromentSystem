import React, { useState, useEffect } from "react";
import {
    CRow,
    CCol,
    CCard,
    CCardBody,
    CFormSelect,
    CCardHeader,
} from "@coreui/react";
import { CChartLine } from "@coreui/react-chartjs";
import axios from "axios";
import ReportSection from '../report/ReportSection';

const WaterConsumption = () => {
    const [year, setYear] = useState(new Date().getFullYear());
    const [chartData, setChartData] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [theme, setTheme] = useState("light");

    const fetchWaterData = async () => {
        setLoading(true);
        setError(null);

        try {
            const response = await axios.get("http://localhost:5154/api/Water");
            const waters = response.data.waters;

            // Filter data for the selected year
            const filteredData = waters.filter(
                (entry) => new Date(entry.date).getFullYear() === year
            );

            // Group data by month
            const monthlyConsumption = Array(12).fill(0);
            filteredData.forEach((entry) => {
                const month = new Date(entry.date).getMonth(); // 0-based index
                monthlyConsumption[month] += entry.usage;
            });

            // Update chart data
            setChartData({
                labels: [
                    "Jan",
                    "Feb",
                    "Mar",
                    "Apr",
                    "May",
                    "Jun",
                    "Jul",
                    "Aug",
                    "Sep",
                    "Oct",
                    "Nov",
                    "Dec",
                ],
                datasets: [
                    {
                        label: `Water Consumption (${year})`,
                        borderColor: "#007bff",
                        backgroundColor: "rgba(0, 123, 255, 0.1)",
                        data: monthlyConsumption,
                        fill: true,
                    },
                ],
            });

            setLoading(false);
        } catch (error) {
            console.error("Error fetching water data:", error);
            setError("Failed to load water data.");
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchWaterData();
    }, [year]);

    useEffect(() => {
        // Detect CoreUI theme (you can customize this as per your theme detection logic)
        const rootElement = document.documentElement;
        const currentTheme = rootElement.getAttribute("data-theme");
        setTheme(currentTheme || "light");
    }, []);

    return (
        <div>
            <ReportSection type="water" />

            <CCard>
                <CCardHeader>
                    <CRow className="mb-3">
                        <CCol xs="auto">
                            <h4>Water Consumption</h4>
                        </CCol>
                        
                        <CCol xs="auto">
                            <CFormSelect
                                className="w-auto"
                                value={year}
                                onChange={(e) => setYear(parseInt(e.target.value))}
                            >
                                {Array.from({ length: 10 }, (_, i) => new Date().getFullYear() - i).map(
                                    (yearOption) => (
                                        <option key={yearOption} value={yearOption}>
                                            {yearOption}
                                        </option>
                                    )
                                )}
                            </CFormSelect>
                        </CCol>
                    </CRow>
                </CCardHeader>
                <CCardBody>
                    {loading ? (
                        <p>Loading water data...</p>
                    ) : error ? (
                        <p className="text-danger">{error}</p>
                    ) : (
                        <CChartLine
                            data={chartData}
                            options={{
                                responsive: true,
                                maintainAspectRatio: false,
                                plugins: {
                                    legend: {
                                        labels: {
                                            color: theme === "dark" ? "#fff" : "#000",
                                        },
                                    },
                                },
                                scales: {
                                    x: {
                                        ticks: {
                                            color: theme === "dark" ? "#fff" : "#000",
                                        },
                                    },
                                    y: {
                                        ticks: {
                                            color: theme === "dark" ? "#fff" : "#000",
                                        },
                                    },
                                },
                            }}
                            style={{ height: "400px" }}
                        />
                    )}
                </CCardBody>
            </CCard>
        </div>
    );
};

export default WaterConsumption;
