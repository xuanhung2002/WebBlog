export const CookiesEventTarget = new EventTarget();
export const clearCookies = () => {
    // Cookies.remove("access_token");
    // Cookies.remove("profile");
    const clearLSEvent = new Event("clearCookies");
    CookiesEventTarget.dispatchEvent(clearLSEvent);
  };