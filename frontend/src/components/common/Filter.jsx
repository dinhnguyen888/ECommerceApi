import PropTypes from "prop-types";
import { Tag } from "antd";
const filters = [
    {
        label: "Phần mềm mới nhất",
        link: "/danh-muc/phanmem",
    },
    {
        label: "Website & App mới nhất",
        link: "/danh-muc/webapp",
    },
    {
        label: "Các Tool % Extension khả dụng",
        link: "/danh-muc/tool",
    },
    {
        label: "Phần mềm Miễn phí",
        link: "/danh-muc/free",
    },
    {
        label: "Tất cả sản phẩm",
        link: "/san-pham",
    },
    {
        label: "Tất cả bài viết",
        link: "/bai-viet",
    },
    {
        label: "Liên hệ",
        link: "https://www.facebook.com/nguyenphucdinh89/",
    },
    {
        label: "Tin tức mới nhất về công nghệ",
        link: "/tin-tuc",
    },
];
function Filter() {
    return (
        <div className="p-2">
            <p className="text-lg font-sans font-semibold text-gray-600 py-4">
                Lọc theo:
            </p>

            <div className="flex flex-wrap gap-4">
                {filters.map((filter, index) => (
                    <Tag
                        key={index}
                        color="blue"
                        className="flex items-center justify-center "
                    >
                        <a
                            href={filter.link}
                            className="text-white w-full text-center"
                        >
                            {filter.label}
                        </a>
                    </Tag>
                ))}
            </div>
        </div>
    );
}

Filter.propTypes = {
    filters: PropTypes.arrayOf(
        PropTypes.shape({
            label: PropTypes.string.isRequired,
            link: PropTypes.string.isRequired,
        })
    ).isRequired,
};

export default Filter;
