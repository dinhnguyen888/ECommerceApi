import { createContext, useState, useContext } from "react";
import PropTypes from "prop-types";
import { viewPaymentHistory } from "../services/payment.viewHistory";
import { CreatePayment } from "../services/payment.create";
import { useNavigate } from "react-router-dom";

const PaymentContext = createContext();

export const PaymentProvider = ({ children }) => {
    const navigate = useNavigate();
    const [paymentHistory, setPaymentHistory] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [tempProductId, setTempProductId] = useState(null);
    const [tempProductPay, setTempProductPay] = useState(null);
    const [tempPrice, setTempPrice] = useState(null);

    const fetchPaymentHistory = async (token) => {
        setLoading(true);
        setError(null);

        try {
            const data = await viewPaymentHistory(token);
            setPaymentHistory(Array.isArray(data) ? data : []);
        } catch (error) {
            setError(error.message);
            setPaymentHistory([]);
        } finally {
            setLoading(false);
        }
    };

    const navigatePayment = (productId, productInfo = null) => {
        if (productInfo) {
            setTempProductId(productId);
            setTempProductPay(productInfo.productPay);
            setTempPrice(productInfo.productPrice);
        }
        navigate(`/checkout/${productId}`);
    };

    const initiatePayment = async (payment, gateway) => {
        setLoading(true);
        setError(null);

        try {
            const response = await CreatePayment(payment, gateway);
            if (response?.data) {
                window.location.href = response.data;
                return response.data;
            }
            throw new Error("Invalid payment response");
        } catch (err) {
            setError(err.message);
            throw err;
        } finally {
            setLoading(false);
        }
    };

    const value = {
        paymentHistory,
        loading,
        error,
        initiatePayment,
        navigatePayment,
        tempProductId,
        tempProductPay,
        tempPrice,
        fetchPaymentHistory,
    };

    return (
        <PaymentContext.Provider value={value}>
            {children}
        </PaymentContext.Provider>
    );
};

PaymentProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export const usePayment = () => {
    const context = useContext(PaymentContext);
    if (!context) {
        throw new Error("usePayment must be used within a PaymentProvider");
    }
    return context;
};
