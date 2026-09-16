import { useLocation } from "react-router-dom";
import PaymentSuccessContent from "../../components/contents/PaymentSuccessContent";
import BaseLayout from "../../components/layout/BaseLayout";

const PaymentSuccessPage = () => {
    const location = useLocation();
    const { transactionId, amount, productName } = location.state || {};

    return (
        <BaseLayout>
            <PaymentSuccessContent
                transactionId={transactionId}
                amount={amount}
                productName={productName}
            />
        </BaseLayout>
    );
};

export default PaymentSuccessPage;
