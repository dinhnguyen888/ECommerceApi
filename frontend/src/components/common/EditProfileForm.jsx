import PropTypes from "prop-types";
import { useState } from "react";
import { Button } from "antd";
import { updateProfile } from "../../services/profile.update";
import { useAuth } from "../../contexts/AuthContext";
import toast from "react-hot-toast";

const EditProfileForm = ({ userProfile, setUserProfile }) => {
    const { accessToken, username, email } = useAuth();
    const [formValues, setFormValues] = useState({
        username: userProfile.username,
        email: userProfile.email,
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormValues((prevValues) => ({ ...prevValues, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const updatedProfile = await updateProfile(
                accessToken,
                formValues.username || username,
                formValues.email || email
            );
            setUserProfile({
                ...userProfile,
                username: updatedProfile.username,
                email: updatedProfile.email,
            });
            toast.success("chỉnh sửa thông tin thành công");
        } catch (error) {
            console.error("Failed to update profile:", error);
        }
    };

    const renderInputField = (label, name, type = "text") => (
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
            {renderInputField("Tên người dùng", "username")}
            {renderInputField("Email", "email", "email")}
            <br />
            {/* <Button
                type="primary"
                size="small"
                className="w-full mb-4"
                onClick={() => toast.success("This is a success message!")}
            >
                Chỉnh sửa ảnh đại diện
            </Button> */}
            <Button
                type="primary"
                size="small"
                htmlType="submit"
                className="w-full"
            >
                Lưu thay đổi
            </Button>
        </form>
    );
};

EditProfileForm.propTypes = {
    userProfile: PropTypes.shape({
        username: PropTypes.string,
        email: PropTypes.string,
    }).isRequired,
    setUserProfile: PropTypes.func.isRequired,
};

export default EditProfileForm;
