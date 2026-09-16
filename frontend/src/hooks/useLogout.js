import { useCallback } from "react";

const useLogout = () => {
    const logout = useCallback(() => {
        // Clear localStorage
        localStorage.clear();

        // Clear cookies
        document.cookie.split(";").forEach((c) => {
            document.cookie = c
                .replace(/^ +/, "")
                .replace(
                    /=.*/,
                    "=;expires=" + new Date().toUTCString() + ";path=/"
                );
        });
    }, []);

    return logout;
};

export default useLogout;
