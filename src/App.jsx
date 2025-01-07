import { useEffect } from 'react'
import './App.css'
import useRootElements from './useRouteElement'
import { CookiesEventTarget } from './services/authService'
import { useAppContext } from './context/AppStore'

function App() {
  const routeElements = useRootElements();
  const {reset} = useAppContext();
  useEffect(() => {
    CookiesEventTarget.addEventListener("clearCookies", reset);
    return () => {
      CookiesEventTarget.removeEventListener("clearCookies", reset);
    };
  }, [reset])
  return <div className='header-frame' style={{position:'fixed',top:'0',left:'0',width:'100%'}}>{routeElements}</div>;
}

export default App
