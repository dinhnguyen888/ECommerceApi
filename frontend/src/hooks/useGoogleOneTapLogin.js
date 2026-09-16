import { useGoogleOneTapLogin as useGoogleOneTapLoginLib } from "react-google-one-tap-login";
import { useAuth } from "../contexts/AuthContext";
import apiClient from "../services/APIConfig";

export const useGoogleOneTapLogin = () => {
    const GOOGLE_API = import.meta.env.VITE_GOOGLE_API;
    const { login, isLoggedIn } = useAuth();
    useGoogleOneTapLoginLib({
        onSuccess: async (response) => {
            console.log("Google login success:", response);
            try {
                const serverResponse = await apiClient.post(
                    "OAuth/login-google-one-tap",
                    response,
                    {
                        headers: {
                            "Content-Type": "application/json",
                        },
                    }
                );
                console.log("Server response:", serverResponse.data);
                if (serverResponse.data) {
                    console.log("Account:", serverResponse.data.accessToken);
                    login(serverResponse.data.accessToken);
                    window.location.href = "/";
                }
            } catch (error) {
                console.error("Error sending data to server:", error);
            }
        },
        onError: (error) => {
            console.log("Google login error:", error);
        },
        disableCancelOnUnmount: false,
        disabled: isLoggedIn,
        googleAccountConfigs: {
            client_id: GOOGLE_API,
        },
    });
};
