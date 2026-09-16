import PropTypes from "prop-types";

function Image({ src, alt, width, height, onClickImage }) {
    return (
        <img
            src={src}
            alt={alt}
            width={width}
            height={height}
            onClick={onClickImage}
            className="cursor-pointer object-cover"
        />
    );
}

Image.propTypes = {
    src: PropTypes.string.isRequired,
    alt: PropTypes.string.isRequired,
    width: PropTypes.number,
    height: PropTypes.number,
    onClickImage: PropTypes.func,
};

export default Image;
