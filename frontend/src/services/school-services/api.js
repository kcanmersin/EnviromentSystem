import axios from 'axios';

const API_BASE_URL = 'http://localhost:5154/api/SchoolInfo';

export const fetchSchoolInfo = async () => {
  const response = await axios.get(API_BASE_URL);
  return response.data.value.schoolInfos;
};

export const updateSchoolInfo = async (data) => {
    data = {
        ...data,
        month : "string",   
    };
  return axios.put(API_BASE_URL, data);
};

export const deleteSchoolInfo = async (id) => {
  return axios.delete(`${API_BASE_URL}/${id}`);
};
