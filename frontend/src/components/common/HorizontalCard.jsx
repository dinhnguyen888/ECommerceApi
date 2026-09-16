import PropTypes from "prop-types";

const HorizontalCard = ({
    title,
    description,
    imageUrl,
    tag,
    postedDate,
    onViewDetails,
}) => {
    return (
        <div
            className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300 cursor-pointer overflow-hidden h-full"
            onClick={onViewDetails}
        >
            <div className="flex flex-col h-full">
                <div className="relative">
                    <img
                        src={imageUrl}
                        alt={title}
                        className="w-full h-48 object-cover"
                        onError={(e) => {
                            e.target.src = "/default-news-image.jpg";
                        }}
                    />
                    {tag && (
                        <span className="absolute top-2 left-2 bg-blue-500 text-white px-2 py-1 rounded-full text-xs">
                            {tag}
                        </span>
                    )}
                </div>

                <div className="p-4 flex flex-col flex-grow">
                    <h2 className="text-xl font-semibold text-gray-800 mb-2 line-clamp-2 hover:text-blue-600">
                        {title}
                    </h2>
                    <p className="text-gray-600 text-sm mb-4 line-clamp-3 flex-grow">
                        {description}
                    </p>
                    <p className="text-gray-400 text-xs">
                        {new Date(postedDate).toLocaleDateString()}
                    </p>
                </div>
            </div>
        </div>
    );
};

HorizontalCard.propTypes = {
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    imageUrl: PropTypes.string.isRequired,
    tag: PropTypes.string,
    postedDate: PropTypes.string.isRequired,
    onViewDetails: PropTypes.func.isRequired,
};

export default HorizontalCard;
