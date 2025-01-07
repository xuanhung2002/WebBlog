import { Outlet, Navigate, useRoutes } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import AuthLayout from './layouts/AuthLayout';
import RegisterPage from './pages/RegisterPage';
import LoginPage from './pages/loginPage';

function ProtectedRoute() {
    const isAuthenticated = true; // need implement
    return isAuthenticated ? <Outlet /> : <Navigate to="/sign-in" />;
}

function RejectedRoute() {
    // Logic cho RejectedRoute
}

function AdminRoute() {
    // check if isAuthenticated and isAdmin
    return <Outlet />;
}

function OpenRoute() {
    return <Outlet />;
}

export default function useRootElements() {
    const routeElements = useRoutes([
        {
            path: "",
            element: <OpenRoute />,
            children: [
                {
                    path: "",
                    element: <MainLayout />,
                    children: [
                    
                    ]
                },
                {
                    path: "/auth",
                    element: <AuthLayout />,
                    children: [
                        {
                            path: "sign-in",
                            element: <LoginPage />
                        },
                        {
                            path: "sign-up",
                            element: <RegisterPage />
                        }
                    ]
                }
            ]
        },
        {
            path: "/",
            element: <ProtectedRoute />,
            children: [
                
            ]
        },


        // ===================================================//
        {
            path: "/admin",
            element: <AdminRoute />,
            children: [
                // admin pages
                {
                    path: "",
                    element: <>Admin Dashboard</>

                }
            ]
        }
    ]);

    return routeElements;
}
