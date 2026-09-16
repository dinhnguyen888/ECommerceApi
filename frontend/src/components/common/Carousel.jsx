import { useState } from "react";
import PropTypes from "prop-types";

const Carousel = ({ images }) => {
    const [selectedImage, setSelectedImage] = useState(images[0]);
    const [fadeClass, setFadeClass] = useState("opacity-100");

    const handleImageChange = (image) => {
        setFadeClass("opacity-0");
        setTimeout(() => {
            setSelectedImage(image);
            setFadeClass("opacity-100");
        }, 300);
    };

    return (
        <div className="flex flex-col items-start justify-center space-y-2 md:space-y-4">
            <div className="relative w-full h-[200px] md:h-64">
                <img
                    className={`h-full w-full object-cover rounded-sm transition-opacity duration-500 ease-in-out ${fadeClass}`}
                    src={selectedImage}
                    alt="Selected"
                />
            </div>
            <div className="flex gap-1 md:gap-2 overflow-x-auto w-full pb-2">
                {images.map((image, index) => (
                    <img
                        key={index}
                        className={`w-12 h-12 md:w-16 md:h-16 rounded-lg cursor-pointer transition-transform duration-300 ease-in-out transform flex-shrink-0 object-fit ${
                            selectedImage === image
                                ? "border-2 border-blue-500"
                                : ""
                        }`}
                        src={image}
                        alt={`Thumbnail ${index + 1}`}
                        onClick={() => handleImageChange(image)}
                    />
                ))}
            </div>
        </div>
    );
};

Carousel.propTypes = {
    images: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default Carousel;
