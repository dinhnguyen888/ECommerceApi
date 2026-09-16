import { BrowserRouter, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import ProductPage from "./pages/ProductPage";
import ProductDetailPage from "./pages/ProductDetailPage";
import CheckoutPage from "./pages/CheckoutPage";
import IntroductionPage from "./pages/IntroductionPage";
import { useGoogleOneTapLogin } from "./hooks/useGoogleOneTapLogin";
import { AuthProvider } from "./contexts/AuthContext";
import { CartProvider } from "./contexts/CartContext";
import { MenuProvider } from "./contexts/MenuContext";
import { PaymentProvider } from "./contexts/PaymentContext";
import SearchPage from "./pages/SearchPage";
import { ProductProvider } from "./contexts/ProductContext";
import SoftwarePage from "./pages/SoftwarePage";
import WebappPage from "./pages/WebappPage";
import ToolPage from "./pages/ToolPage";
import OAuthCallback from "./pages/auth/OAuthCallback";
import PaymentSuccessPage from "./pages/payment/PaymentSuccessPage";
import PaymentFailurePage from "./pages/payment/PaymentFailurePage";
import ArticlePage from "./pages/ArticlePage";
import NewsPage from "./pages/NewsPage";
import FreePage from "./pages/FreePage";

function App() {
    useGoogleOneTapLogin();

    return (
        <BrowserRouter>
            <ProductProvider>
                <AuthProvider>
                    <CartProvider>
                        <MenuProvider>
                            <PaymentProvider>
                                <Routes>
                                    <Route path="/" element={<HomePage />} />
                                    <Route
                                        path="/dang-nhap"
                                        element={<LoginPage />}
                                    />
                                    <Route
                                        path="/dang-ky"
                                        element={<RegisterPage />}
                                    />
                                    <Route
                                        path="/san-pham"
                                        element={<ProductPage />}
                                    />
                                    <Route
                                        path="/gioi-thieu"
                                        element={<IntroductionPage />}
                                    />
                                    <Route
                                        path="/san-pham/:productId"
                                        element={<ProductDetailPage />}
                                    />
                                    <Route
                                        path="/checkout/:productId"
                                        element={<CheckoutPage />}
                                    />
                                    <Route
                                        path="/search"
                                        element={<SearchPage />}
                                    />
                                    <Route
                                        path="danh-muc/phanmem"
                                        element={<SoftwarePage />}
                                    />
                                    <Route
                                        path="/danh-muc/webapp"
                                        element={<WebappPage />}
                                    />
                                    <Route
                                        path="danh-muc/tool"
                                        element={<ToolPage />}
                                    />
                                    <Route
                                        path="/oauth/callback"
                                        element={<OAuthCallback />}
                                    />
                                    <Route
                                        path="/thanh-toan-thanh-cong"
                                        element={<PaymentSuccessPage />}
                                    />
                                    <Route
                                        path="/thanh-toan-that-bai"
                                        element={<PaymentFailurePage />}
                                    />
                                    <Route
                                        path="/bai-viet"
                                        element={<ArticlePage />}
                                    />
                                    <Route
                                        path="/tin-tuc"
                                        element={<NewsPage />}
                                    />
                                    <Route
                                        path="danh-muc/free"
                                        element={<FreePage />}
                                    />
                                </Routes>
                            </PaymentProvider>
                        </MenuProvider>
                    </CartProvider>
                </AuthProvider>
            </ProductProvider>
        </BrowserRouter>
    );
}

export default App;
