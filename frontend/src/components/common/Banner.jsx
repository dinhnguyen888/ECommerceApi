import PropTypes from "prop-types";
import { Carousel } from "antd";

const Banner = ({ images }) => {
    return (
        <Carousel autoplay>
            {images.map((image, index) => (
                <div
                    key={index}
                    className="relative rounded-md shadow-sm overflow-hidden"
                >
                    <img
                        src={image.bannerUrl}
                        alt={image.bannerName}
                        className="w-full lg:h-[300px] h-[200px] object-fit"
                    />

                    {/* understand html data
                    {overlayTexts && overlayTexts[index] && (
                        <div className="absolute inset-y-0 left-0 w-1/3 my-11 mx-4 ">
                            <div
                                dangerouslySetInnerHTML={{
                                    __html: overlayTexts[index],
                                }}
                            />
                        </div>
                    )} */}
                </div>
            ))}
        </Carousel>
    );
};

Banner.propTypes = {
    images: PropTypes.arrayOf(
        PropTypes.shape({
            bannerUrl: PropTypes.string.isRequired,
            bannerName: PropTypes.string.isRequired,
        })
    ).isRequired,
    // overlayTexts: PropTypes.arrayOf(PropTypes.string),
};

export default Banner;
