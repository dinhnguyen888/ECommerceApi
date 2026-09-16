import PropTypes from "prop-types";
import { Card, Button } from "antd";

const RelativeThing = ({ items }) => {
    return (
        <div className="py-7">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:gap-8">
                {items.slice(0, 6).map((item, index) => (
                    <Card
                        key={index}
                        hoverable
                        className="rounded-sm overflow-hidden border-gray-500 hover:shadow-sm transition-shadow"
                        onClick={() =>
                            (window.location.href = `/san-pham/${item.id}`)
                        }
                    >
                        <div className="flex p-0 ">
                            {/* Bên trái: Hình ảnh tràn hết */}
                            <div className="w-1/3">
                                <img
                                    alt={item.title}
                                    src={item.imageUrl}
                                    className="w-full h-full object-cover"
                                />
                            </div>

                            {/* Bên phải: Nội dung */}
                            <div className="w-2/3 p-4 flex flex-col justify-between">
                                <div>
                                    <h3 className="text-lg font-semibold text-gray-800">
                                        {item.title}
                                    </h3>
                                    {item.price && (
                                        <p className="mt-1 text-lg font-semibold text-green-600">
                                            {item.price}
                                        </p>
                                    )}
                                </div>
                                <Button
                                    type="link"
                                    className="text-blue-500 font-medium hover:text-blue-700 float-right"
                                >
                                    Xem chi tiết
                                </Button>
                            </div>
                        </div>
                    </Card>
                ))}
            </div>
        </div>
    );
};

RelativeThing.propTypes = {
    items: PropTypes.arrayOf(
        PropTypes.shape({
            imageUrl: PropTypes.string.isRequired,
            title: PropTypes.string.isRequired,
            price: PropTypes.string,
        })
    ).isRequired,
};

export default RelativeThing;
