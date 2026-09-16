import PropTypes from "prop-types";
import { Table } from "antd";

const CustomTable = ({ data, className, isPagination, buttons }) => {
    const columns = Object.keys(data[0]).map((key) => ({
        title: key.charAt(0).toUpperCase() + key.slice(1),
        dataIndex: key,
        key: key,
        align: "center",
        render: (text) => (
            <span>
                {text.split(",").map((item, index) => (
                    <span key={index} className={"block"}>
                        {item.trim()}
                    </span>
                ))}
            </span>
        ),
    }));

    return (
        <div className="relative">
            <Table
                columns={columns}
                dataSource={data}
                className={className}
                pagination={isPagination}
            />
            <div className="absolute top-0 right-0 flex space-x-2 p-2">
                {buttons &&
                    buttons.map((button, index) => (
                        <div key={index}>{button}</div>
                    ))}
            </div>
        </div>
    );
};

CustomTable.propTypes = {
    data: PropTypes.arrayOf(PropTypes.object).isRequired,
    className: PropTypes.string,
    isPagination: PropTypes.bool,
    buttons: PropTypes.arrayOf(PropTypes.node),
};

export default CustomTable;
