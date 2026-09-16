import {
    BrowserRouter,
    Routes,
    Route,
    Navigate,
    useLocation,
} from "react-router-dom";
import { LoginPage } from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import { ProductPage } from "./pages/ProductPage";
import { PaymentPage } from "./pages/PaymentPage";
import { AccountPage } from "./pages/AccountPage";
import { RolePage } from "./pages/RolePage";
import NewsPage from "./pages/NewsPage";
import CommentPage from "./pages/CommentPage";
import { BannerPage } from "./pages/BannerPage";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { App as AntApp } from "antd";
import { ReactNode } from "react";

interface ProtectedRouteProps {
    children: ReactNode;
}

const ProtectedRoute = ({ children }: ProtectedRouteProps) => {
    const { isAuthenticated } = useAuth();
    const location = useLocation();

    if (!isAuthenticated) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    return <>{children}</>;
};

const AppRoutes = () => {
    const { isAuthenticated } = useAuth();

    return (
        <Routes>
            {/* Public routes */}
            <Route
                path="/login"
                element={
                    isAuthenticated ? (
                        <Navigate to="/dashboard" />
                    ) : (
                        <LoginPage />
                    )
                }
            />

            {/* Protected routes */}
            <Route
                path="/dashboard"
                element={
                    <ProtectedRoute>
                        <DashboardPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/products"
                element={
                    <ProtectedRoute>
                        <ProductPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/products/:productId/comments"
                element={
                    <ProtectedRoute>
                        <CommentPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/banners"
                element={
                    <ProtectedRoute>
                        <BannerPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/payments"
                element={
                    <ProtectedRoute>
                        <PaymentPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/accounts"
                element={
                    <ProtectedRoute>
                        <AccountPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/roles"
                element={
                    <ProtectedRoute>
                        <RolePage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/news"
                element={
                    <ProtectedRoute>
                        <NewsPage />
                    </ProtectedRoute>
                }
            />

            {/* Redirect root to dashboard if authenticated, otherwise to login */}
            <Route
                path="/"
                element={
                    <Navigate to={isAuthenticated ? "/dashboard" : "/login"} />
                }
            />

            {/* Catch all route */}
            <Route path="*" element={<Navigate to="/" />} />
        </Routes>
    );
};

const App = () => {
    return (
        <AntApp>
            <AuthProvider>
                <BrowserRouter>
                    <AppRoutes />
                </BrowserRouter>
            </AuthProvider>
        </AntApp>
    );
};

export default App;
