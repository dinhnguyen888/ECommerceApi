import PropTypes from "prop-types";
import { useState } from "react";
import { useAuth } from "../../contexts/AuthContext";
import EditProfileForm from "./EditProfileForm";
import ChangePasswordForm from "./ChangePasswordForm";

const Profile = () => {
    const { username, email } = useAuth();
    const [userProfile, setUserProfile] = useState({
        avatarUrl: "https://placehold.co/100x100",
        username,
        email,
        joinDate: "02/01/2024",
    });

    const [activeTab, setActiveTab] = useState(null);

    return (
        <div className="max-w-4xl mx-auto bg-white shadow-lg overflow-hidden">
            <div className="flex flex-col md:flex-row p-6">
                <div className="w-full md:w-1/3 text-center mb-4 md:mb-0">
                    <img
                        src={userProfile.avatarUrl}
                        alt={userProfile.username}
                        className="w-24 h-24 rounded-full mx-auto object-cover"
                    />
                    <p className="mt-2 text-xl text-black font-semibold">
                        {userProfile.username}
                    </p>
                    <p className="text-gray-600">{userProfile.email}</p>
                    <p className="text-gray-600">
                        Ngày gia nhập: {userProfile.joinDate}
                    </p>
                </div>
                <div className="w-full md:w-2/3">
                    <div className="flex justify-end space-x-4">
                        <button
                            className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                            onClick={() => setActiveTab("editInfo")}
                        >
                            Chỉnh sửa thông tin
                        </button>
                        <button
                            className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
                            onClick={() => setActiveTab("changePassword")}
                        >
                            Đổi mật khẩu
                        </button>
                    </div>
                </div>
            </div>
            <div className="mx-4 my-4">
                {activeTab === "editInfo" && (
                    <EditProfileForm
                        userProfile={userProfile}
                        setUserProfile={setUserProfile}
                    />
                )}
                {activeTab === "changePassword" && <ChangePasswordForm />}
            </div>
        </div>
    );
};

Profile.propTypes = {
    userProfile: PropTypes.shape({
        avatarUrl: PropTypes.string,
        username: PropTypes.string,
        email: PropTypes.string,
        joinDate: PropTypes.string,
    }),
};

export default Profile;
