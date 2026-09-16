import { Menu } from "antd";
import {
    ShoppingCartOutlined,
    UnorderedListOutlined,
    UserOutlined,
    LogoutOutlined,
} from "@ant-design/icons";
import PropTypes from "prop-types";

export default function MenuNavbar({ activeComponent, onMenuClick }) {
    return (
        <Menu
            theme="dark"
            mode="horizontal"
            selectedKeys={[activeComponent]}
            onClick={onMenuClick}
            className="w-full border-b-0"
        >
            <Menu.Item
                key="Cart"
                icon={<ShoppingCartOutlined />}
                className="hover:bg-gray-700 hover:text-white"
            >
                Giỏ hàng
            </Menu.Item>
            <Menu.Item
                key="PaymentBill"
                icon={<UnorderedListOutlined />}
                className="hover:bg-gray-700 hover:text-white"
            >
                Đơn mua
            </Menu.Item>
            <Menu.Item
                key="Profile"
                icon={<UserOutlined />}
                className="hover:bg-gray-700 hover:text-white"
            >
                Tài khoản
            </Menu.Item>
            <Menu.Item
                key="logout"
                icon={<LogoutOutlined />}
                className="hover:bg-red-600 hover:text-white"
            >
                Đăng xuất
            </Menu.Item>
        </Menu>
    );
}

MenuNavbar.propTypes = {
    activeComponent: PropTypes.string.isRequired,
    onMenuClick: PropTypes.func.isRequired,
};
