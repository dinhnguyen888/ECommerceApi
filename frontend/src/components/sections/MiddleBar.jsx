import Navbar from "../common/Navbar";
import { useAuth } from "../../contexts/AuthContext";
import { useMenu } from "../../contexts/MenuContext";

function MiddleBar() {
    const { isLoggedIn } = useAuth();
    const { toggleMenu } = useMenu();

    const handleOpenMenu = () => {
        toggleMenu();
    };

    return (
        <div className="sticky top-0 left-0 w-full z-50">
            <Navbar
                links={[
                    { href: "/", text: "Trang chủ" },
                    { href: "/danh-muc/phanmem", text: "Phần mềm" },
                    { href: "/danh-muc/webapp", text: "Web App" },
                    { href: "/danh-muc/tool", text: "Tool & Data" },
                    {
                        href: "https://github.com/dinhnguyen888",
                        text: "Giới thiệu",
                    },
                    {
                        href: isLoggedIn ? null : "/dang-nhap",
                        text: isLoggedIn ? "Menu ☰" : "Đăng nhập🔑",
                        onClick: isLoggedIn ? handleOpenMenu : null,
                    },
                ]}
            />
        </div>
    );
}

export default MiddleBar;
