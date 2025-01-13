// src/store/authSlice.js
import { createSlice } from '@reduxjs/toolkit'

// Read initial auth state from local storage
const userData = JSON.parse(localStorage.getItem('user'))

const initialState = {
  isAuthenticated: userData ? true : false,
  user: userData || null,
}

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginSuccess: (state, action) => {
      state.isAuthenticated = true
      state.user = action.payload
      localStorage.setItem('user', JSON.stringify(action.payload)) // ✅ Save to localStorage
    },
    logout: (state) => {
      state.isAuthenticated = false
      state.user = null
      localStorage.removeItem('user') // ❌ Remove from localStorage
    },
  },
})

export const { loginSuccess, logout } = authSlice.actions
export default authSlice.reducer
