import PropTypes from "prop-types";
import { Button } from "antd";
import {
    UserOutlined,
    ShoppingOutlined,
    CreditCardOutlined,
} from "@ant-design/icons";

const PaymentMethod = ({
    value,
    selected,
    onSelect,
    logo,
    name,
    isSandboxEnv,
}) => (
    <div
        className={`bg-white p-4 rounded-lg shadow-sm hover:shadow-md transition-all cursor-pointer relative 
            ${selected ? "ring-2 ring-blue-500 shadow-lg bg-blue-50" : ""}`}
        onClick={() => onSelect(value)}
    >
        <div
            className={`flex items-center justify-center space-x-3 relative transition-transform ${
                selected ? "scale-110" : ""
            }`}
        >
            <img src={logo} alt={name} className="w-10 h-10" />
            <span className={`font-medium ${selected ? "text-blue-600" : ""}`}>
                {name}
            </span>
            {isSandboxEnv && (
                <div className="text-xs float-right absolute top-0 right-0 text-gray-500">
                    (Testing)
                </div>
            )}
        </div>
    </div>
);

PaymentMethod.propTypes = {
    value: PropTypes.string.isRequired,
    selected: PropTypes.bool.isRequired,
    onSelect: PropTypes.func.isRequired,
    logo: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    isSandboxEnv: PropTypes.bool,
};

const CheckoutContent = ({
    username,
    email,
    productName,
    productPrice,
    selectedGateway,
    onGatewayChange,
    onPayment,
    loading,
    paymentMethods,
}) => {
    return (
        <div className="max-w-3xl mx-auto p-8 bg-white rounded-xl shadow-lg">
            <h2 className="text-3xl font-bold mb-8 text-center text-gray-800">
                Xác nhận thanh toán
            </h2>

            <div className="space-y-8">
                {/* Thông tin người mua */}
                <div className="bg-blue-50 rounded-lg p-6">
                    <div className="flex items-center mb-4">
                        <UserOutlined className="text-2xl text-blue-500 mr-2" />
                        <h3 className="text-xl font-semibold text-gray-800">
                            Thông tin người mua
                        </h3>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="bg-white p-4 rounded-lg shadow-sm">
                            <p className="text-gray-500 text-sm">Họ và tên</p>
                            <p className="font-medium">{username}</p>
                        </div>
                        <div className="bg-white p-4 rounded-lg shadow-sm">
                            <p className="text-gray-500 text-sm">Email</p>
                            <p className="font-medium">{email}</p>
                        </div>
                    </div>
                </div>

                {/* Thông tin sản phẩm */}
                <div className="bg-green-50 rounded-lg p-6">
                    <div className="flex items-center mb-4">
                        <ShoppingOutlined className="text-2xl text-green-500 mr-2" />
                        <h3 className="text-xl font-semibold text-gray-800">
                            Thông tin sản phẩm
                        </h3>
                    </div>
                    <div className="bg-white p-4 rounded-lg shadow-sm">
                        <div className="flex justify-between items-center">
                            <div>
                                <p className="text-gray-500 text-sm">
                                    Tên sản phẩm
                                </p>
                                <p className="font-medium">{productName}</p>
                            </div>
                            <div className="text-right">
                                <p className="text-gray-500 text-sm">
                                    Giá tiền
                                </p>
                                <p className="text-xl font-bold text-red-500">
                                    {productPrice?.toLocaleString()}đ
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Phương thức thanh toán */}
                <div className="bg-purple-50 rounded-lg p-6">
                    <div className="flex items-center mb-4">
                        <CreditCardOutlined className="text-2xl text-purple-500 mr-2" />
                        <h3 className="text-xl font-semibold text-gray-800">
                            Phương thức thanh toán
                        </h3>
                    </div>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        {paymentMethods.map((method) => (
                            <PaymentMethod
                                key={method.value}
                                value={method.value}
                                selected={selectedGateway === method.value}
                                onSelect={onGatewayChange}
                                logo={method.logo}
                                name={method.name}
                                isSandboxEnv={method.isSandboxEnv}
                            />
                        ))}
                    </div>
                </div>
            </div>

            <Button
                type="primary"
                size="large"
                block
                onClick={onPayment}
                loading={loading}
                className="mt-8 h-12 text-lg bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700"
            >
                Xác nhận thanh toán
            </Button>
        </div>
    );
};

CheckoutContent.propTypes = {
    username: PropTypes.string.isRequired,
    email: PropTypes.string.isRequired,
    productName: PropTypes.string.isRequired,
    productPrice: PropTypes.number.isRequired,
    selectedGateway: PropTypes.string.isRequired,
    onGatewayChange: PropTypes.func.isRequired,
    onPayment: PropTypes.func.isRequired,
    loading: PropTypes.bool.isRequired,
    paymentMethods: PropTypes.arrayOf(
        PropTypes.shape({
            value: PropTypes.string.isRequired,
            name: PropTypes.string.isRequired,
            logo: PropTypes.string.isRequired,
            isSandboxEnv: PropTypes.bool,
        })
    ).isRequired,
};

export default CheckoutContent;
