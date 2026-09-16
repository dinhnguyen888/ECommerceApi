import { useLocation } from "react-router-dom";
import PaymentFailureContent from "../../components/contents/PaymentFailureContent";
import BaseLayout from "../../components/layout/BaseLayout";

const PaymentFailurePage = () => {
    const location = useLocation();
    const { errorMessage, amount, productName } = location.state || {};

    return (
        <BaseLayout>
            <PaymentFailureContent
                errorMessage={errorMessage}
                amount={amount}
                productName={productName}
            />
        </BaseLayout>
    );
};

export default PaymentFailurePage;
