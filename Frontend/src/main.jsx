import { BrowserRouter } from "react-router-dom";
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { AppProvider } from "./context/AppStore.jsx";
import { ToastContainer } from "react-toastify";

createRoot(document.getElementById("root")).render(
  <AppProvider>
    <BrowserRouter>
    <ToastContainer/>
      <App />
    </BrowserRouter>
  </AppProvider>
);
