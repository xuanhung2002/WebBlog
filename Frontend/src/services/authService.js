
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
    else{
      console.log("call myprofile failed");
      return null;
    }
  }

  export const getProfileFromSessionStorage = () => {

    const result = sessionStorage.getItem("myProfile");
    // console.log("profileee", JSON.parse(result));
    return result ? JSON.parse(result) : null;
  };


  export const refreshAccessToken = async () => {
      try {
        await apiService.post("/api/auth/refresh-token");
      } catch (error) {
        console.error("Refresh token failed", error);
        throw error;
      }
    };

    
  