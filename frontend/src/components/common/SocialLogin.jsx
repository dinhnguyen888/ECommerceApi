import { Button } from "antd";
import { Github, Chrome } from "lucide-react";
import PropTypes from "prop-types";

const SocialLogin = ({ onGoogleClick, onGithubClick }) => (
    <div className="mt-6">
        <div className="relative mb-6">
            <div className="absolute inset-0 flex items-center">
                <div className="w-full border-t border-gray-300"></div>
            </div>
            <div className="relative flex justify-center text-sm">
                <span className="px-2 text-gray-500 bg-white">
                    Hoặc đăng nhập với
                </span>
            </div>
        </div>
        <div className="grid grid-cols-2 gap-4">
            <Button
                type="default"
                htmlType="button"
                className="flex items-center justify-center h-11 px-6 transition-colors duration-200 ease-in-out hover:bg-red-50"
                style={{
                    borderColor: "#DB4437",
                    color: "#DB4437",
                }}
                icon={<Chrome className="mr-2" size={18} />}
                onClick={onGoogleClick}
            >
                Google
            </Button>
            <Button
                type="default"
                htmlType="button"
                className="flex items-center justify-center h-11 px-6 transition-colors duration-200 ease-in-out hover:bg-gray-50"
                style={{
                    borderColor: "#333",
                    color: "#333",
                }}
                icon={<Github className="mr-2" size={18} />}
                onClick={onGithubClick}
            >
                Github
            </Button>
        </div>
    </div>
);

SocialLogin.propTypes = {
    onGoogleClick: PropTypes.func.isRequired,
    onGithubClick: PropTypes.func.isRequired,
};

export default SocialLogin;
