import React, { useState, useEffect } from "react";
import Select from "react-select";
import { CRow, CCol, CCard, CCardBody, CFormSelect, CCardHeader } from "@coreui/react";
import { CChartLine } from "@coreui/react-chartjs";
import axios from "axios";

const LineChartComparison = ({ buildings, type }) => {
    const [selectedBuildings, setSelectedBuildings] = useState([]);
    const [year, setYear] = useState(new Date().getFullYear());
    const [chartData, setChartData] = useState({});
    const [theme, setTheme] = useState("light");

    const fetchData = async (building) => {
        try {
            const startDate = `${year}-01-01T00:00:00Z`;
            const endDate = `${year}-12-31T23:59:59Z`;

            const apiUrl = `http://localhost:5154/api/${type === "electric" ? "Electric" : "NaturalGas"}`;
            const response = await axios.get(apiUrl, {
                params: {
                    buildingId: building.id,
                    startDate: startDate,
                    endDate: endDate,
                },
            });

            const dataKey = type === "electric" ? "electrics" : "naturalGas";
            const monthlyConsumption = Array(12).fill(0);
            response.data[dataKey].forEach((entry) => {
                const month = new Date(entry.date).getMonth();
                monthlyConsumption[month] += entry.usage;
            });

            return { buildingName: building.label, monthlyData: monthlyConsumption };
        } catch (error) {
            console.error(`Error fetching data for ${building.label}:`, error);
            return null;
        }
    };

    const updateChartData = async () => {
        const dataPromises = selectedBuildings.map((building) => fetchData(building));
        const results = await Promise.all(dataPromises);

        const datasets = results
            .filter((result) => result !== null)
            .map((result, index) => ({
                label: result.buildingName,
                borderColor: `hsl(${(index * 40) % 360}, 70%, 50%)`,
                data: result.monthlyData,
                fill: false,
            }));

        setChartData({
            labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            datasets: datasets,
        });
    };

    const handleBuildingChange = (selectedOptions) => {
        setSelectedBuildings(selectedOptions || []);
    };

    useEffect(() => {
        if (selectedBuildings.length > 0) {
            updateChartData();
        }
    }, [selectedBuildings, year]);

    const customStyles = {
        control: (styles, { isFocused }) => ({
            ...styles,
            backgroundColor: theme === "dark" ? "#2a2a2a" : "#fff",
            color: theme === "dark" ? "#fff" : "#000",
            borderColor: isFocused ? "#007bff" : theme === "dark" ? "#555" : "#ddd",
            boxShadow: isFocused ? "0 0 5px #007bff" : undefined,
        }),
        menu: (styles) => ({
            ...styles,
            backgroundColor: theme === "dark" ? "#2a2a2a" : "#fff",
            color: theme === "dark" ? "#fff" : "#000",
        }),
        singleValue: (styles) => ({
            ...styles,
            color: theme === "dark" ? "#fff" : "#000",
        }),
        multiValue: (styles) => ({
            ...styles,
            backgroundColor: theme === "dark" ? "#555" : "#e0e0e0",
        }),
        multiValueLabel: (styles) => ({
            ...styles,
            color: theme === "dark" ? "#fff" : "#000",
        }),
    };

    useEffect(() => {
        const rootElement = document.documentElement;
        const currentTheme = rootElement.getAttribute("data-theme");
        setTheme(currentTheme || "light");
    }, []);

    return (
        <div>
            <CCard>
                <CCardHeader>
                    <CRow className="mb-3">
                        <CCol xs="auto" md="auto">
                            <h4>{type === "electric" ? "Electricity" : "Natural Gas"} Consumption Comparison</h4>
                        </CCol>

                        <CCol>
                            <Select
                                options={buildings}
                                isMulti
                                onChange={handleBuildingChange}
                                placeholder="Search and select buildings..."
                                className="mb-3"
                                classNamePrefix="react-select"
                                styles={customStyles}
                            />
                        </CCol>

                        <CCol xs="auto" md="auto">
                            <CFormSelect
                                className="w-auto d-inline-block"
                                value={year}
                                onChange={(e) => setYear(parseInt(e.target.value))}
                            >
                                {Array.from({ length: 10 }, (_, i) => new Date().getFullYear() - i).map((year) => (
                                    <option key={year} value={year}>
                                        {year}
                                    </option>
                                ))}
                            </CFormSelect>
                        </CCol>
                    </CRow>
                </CCardHeader>

                <CCardBody>
                    {selectedBuildings.length === 0 ? (
                        <p className="text-center">Select buildings to view data.</p>
                    ) : (
                        <CChartLine
                            data={chartData}
                            options={{
                                responsive: true,
                                maintainAspectRatio: false,
                            }}
                            style={{ height: "400px" }}
                        />
                    )}
                </CCardBody>
            </CCard>
        </div>
    );
};

export default LineChartComparison;
