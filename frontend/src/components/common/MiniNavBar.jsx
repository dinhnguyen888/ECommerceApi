import PropTypes from "prop-types";
import { Menu } from "antd";

function MiniNavBar({ items, onClickActiveBar }) {
    return (
        <Menu
            mode="horizontal"
            className="flex justify-center md:justify-start"
        >
            {items.map((item) => (
                <Menu.Item
                    className=""
                    onClick={() => onClickActiveBar(item.key)}
                    key={item.key}
                >
                    {item.label}
                </Menu.Item>
            ))}
        </Menu>
    );
}

MiniNavBar.propTypes = {
    items: PropTypes.arrayOf(
        PropTypes.shape({
            key: PropTypes.string.isRequired,
            label: PropTypes.string.isRequired,
        })
    ).isRequired,
    onClickActiveBar: PropTypes.func.isRequired,
};

export default MiniNavBar;
