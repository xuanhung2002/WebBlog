import React from 'react'
import Header from '../components/Header/Header'
import { Outlet } from 'react-router-dom'

export default function AuthLayout() {
  return (
    <>
    <Header/>
    <Outlet/>
    </>
  )
}
