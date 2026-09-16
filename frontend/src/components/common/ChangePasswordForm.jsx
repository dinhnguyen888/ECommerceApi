import PropTypes from "prop-types";
import { useState } from "react";
import { changePassword } from "../../services/profile.changePassword";
import { useAuth } from "../../contexts/AuthContext";

const ChangePasswordForm = () => {
    const { accessToken } = useAuth();
    const [formValues, setFormValues] = useState({
        oldPassword: "",
        newPassword: "",
        confirmPassword: "",
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormValues((prevValues) => ({ ...prevValues, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await changePassword(
                accessToken,
                formValues.oldPassword,
                formValues.newPassword
            );
            alert("Password changed successfully");
        } catch (error) {
            console.error("Failed to change password:", error);
        }
    };

    const renderInputField = (label, name, type = "password") => (
        <div className="mb-4">
            <label className="block text-gray-700">{label}</label>
            <input
                type={type}
                name={name}
                value={formValues[name]}
                onChange={handleInputChange}
                className="w-full px-3 py-2 border rounded"
            />
        </div>
    );

    return (
        <form onSubmit={handleSubmit}>
            {renderInputField("Mật khẩu cũ", "oldPassword")}
            {renderInputField("Mật khẩu mới", "newPassword")}
            {renderInputField("Xác nhận mật khẩu", "confirmPassword")}
            <button
                type="submit"
                className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
            >
                Đổi mật khẩu
            </button>
        </form>
    );
};

ChangePasswordForm.propTypes = {
    accessToken: PropTypes.string,
};

export default ChangePasswordForm;
