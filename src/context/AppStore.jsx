import { createContext, useContext, useEffect, useState } from "react";
import { getAccessTokenFromCookies, getMyProfile, getProfileFromSessionStorage } from "../services/authService";
import { use } from "react";
import Cookies from "js-cookie";
import apiService from "../services/apiSerivce";
import { APIS } from "../constants/apis";

const initialAppContext = {
    isAuthenticated: null,
    setIsAuthenticated: () => null,
    profile: null,
    setProfile: () => null,
    reset: () => null,
  };
  
  const AppContext = createContext(initialAppContext);
  
  export const AppProvider = ({ children, defaultValue = initialAppContext }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(
      defaultValue.isAuthenticated
    );
    const [profile, setProfile] = useState(defaultValue.profile);
    
    useEffect(() => {
      var isLogged = Cookies.get("isLogged");
      console.log("isLogged", isLogged);
      if(isLogged === "false" || !isLogged){
        reset()
        console.log("isAuth", isAuthenticated)
      }
      else{

        var sessionProfile = getProfileFromSessionStorage();
        console.log("My profile in context: ", sessionProfile)
        if(!sessionProfile){
          var currentProfile = getMyProfile();
          if(currentProfile){
            setIsAuthenticated(true);
            setProfile(currentProfile);
          }
          else{
            reset();
          }
        }
        else{
          setIsAuthenticated(true)
          setProfile(sessionProfile)
        }
      }      
    }, [])

    const reset = async () => {
      Cookies.set("isLogged", false)
      // await apiService.post(APIS.Logout);
      setIsAuthenticated(false);
      setProfile(null);
    };
  
    return (
      <AppContext.Provider
        value={{
          isAuthenticated,
          setIsAuthenticated,
          profile,
          setProfile,
          reset,
        }}
      >
        {children}
      </AppContext.Provider>
    );
  };
  
  export const AppConsumer = AppContext.Consumer;
  
  export const useAppContext = () => useContext(AppContext);