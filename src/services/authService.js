
import Cookies from "js-cookie";
import apiService from "./apiSerivce";
export const CookiesEventTarget = new EventTarget();
export const clearCookies = () => {
    const clearLSEvent = new Event("clearCookies");
    CookiesEventTarget.dispatchEvent(clearLSEvent);
  };


  export const getAccessTokenFromCookies = () =>{
    var cookie = Cookies.get("accessToken") ? Cookies.get("accessToken") : null;
    console.log("Cokieeee", cookie)
  }
    
  export const getMyProfile = async () => {
    var res = await apiService.get("/api/user/myprofile", {})
    if(res.status === 200 && res.data){
      console.log("Init myprofile: ", res.data)
      return res.data
    }
    return null;
  }

  export const getProfileFromLocalStorage = () => {

    const result = localStorage.getItem("myProfile");
    console.log("profileee", result);
    return result ? JSON.parse(result) : null;
  };