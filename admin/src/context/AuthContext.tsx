import { createContext, useContext, useState, ReactNode } from "react";
import { jwtDecode } from "jwt-decode";
import { JwtPayload } from "../types/auth.type";

interface AuthContextType {
    isAuthenticated: boolean;
    login: (tokens: { accessToken: string; refreshToken: string }) => void;
    logout: () => void;
    accessToken: string | null;
    refreshToken: string | null;
    user: JwtPayload | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [accessToken, setAccessToken] = useState<string | null>(() => {
        return localStorage.getItem("accessToken");
    });
    const [refreshToken, setRefreshToken] = useState<string | null>(() => {
        return localStorage.getItem("refreshToken");
    });
    const [user, setUser] = useState<JwtPayload | null>(() => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            try {
                return jwtDecode(token);
            } catch {
                return null;
            }
        }
        return null;
    });

    const login = (tokens: { accessToken: string; refreshToken: string }) => {
        localStorage.setItem("accessToken", tokens.accessToken);
        localStorage.setItem("refreshToken", tokens.refreshToken);
        setAccessToken(tokens.accessToken);
        setRefreshToken(tokens.refreshToken);
        try {
            const decoded = jwtDecode<JwtPayload>(tokens.accessToken);
            setUser(decoded);
        } catch (error) {
            console.error("Error decoding token:", error);
        }
    };

    const logout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        setAccessToken(null);
        setRefreshToken(null);
        setUser(null);
    };

    const value = {
        isAuthenticated: !!accessToken,
        login,
        logout,
        accessToken,
        refreshToken,
        user,
    };

    return (
        <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};
