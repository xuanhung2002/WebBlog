import { Outlet, Navigate} from 'react-router-dom';

function ProtectedRoute(){
    const isAuthenticated = true; //need implement
    return isAuthenticated ? <Outlet/> : <Navigate to= "/login"/>;
}
function RejectedRoute(){

}

function AdminRoute(){
    
}