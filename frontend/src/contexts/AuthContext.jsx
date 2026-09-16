import { createContext, useState, useContext, useEffect } from "react";
import PropTypes from "prop-types";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [accessToken, setAccessToken] = useState(null);
    const [username, setUsername] = useState(null);
    const [userId, setUserId] = useState(null);
    const [email, setEmail] = useState(null);

    const decodeToken = (token) => {
        try {
            const decoded = jwtDecode(token);
            return {
                email: decoded.email,
                userId: decoded.userId,
                username: decoded.userName,
            };
        } catch (error) {
            console.error("Invalid token:", error);
            return null;
        }
    };

    useEffect(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            setIsLoggedIn(true);
            setAccessToken(token);
            const decodedToken = decodeToken(token);
            if (decodedToken) {
                setUsername(decodedToken.username);
                setUserId(decodedToken.userId);
                setEmail(decodedToken.email);
            }
        }
    }, []);

    const login = (token) => {
        setIsLoggedIn(true);
        setAccessToken(token);
        const decodedToken = decodeToken(token);
        if (decodedToken) {
            setUsername(decodedToken.username);
            setUserId(decodedToken.userId);
            setEmail(decodedToken.email);
        }
        localStorage.setItem("accessToken", token);
    };

    const logout = () => {
        setIsLoggedIn(false);
        setAccessToken(null);
        setUsername(null);
        setUserId(null);
        setEmail(null);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("userInfo");
    };

    return (
        <AuthContext.Provider
            value={{
                isLoggedIn,
                accessToken,
                username,
                userId,
                email,
                login,
                logout,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};
AuthProvider.propTypes = {
    children: PropTypes.node.isRequired,
};
export const useAuth = () => useContext(AuthContext);
