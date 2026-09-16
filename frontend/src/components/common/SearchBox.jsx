import { useState } from "react";
import propTypes from "prop-types";

const SearchBox = ({ handleOnSearch }) => {
    const [isFocused, setIsFocused] = useState(false);
    const [searchValue, setSearchValue] = useState("");

    const handleKeyPress = (event) => {
        if (event.key === "Enter") {
            handleOnSearch(searchValue);
        }
    };

    const handleFocus = () => {
        setIsFocused(true);
    };

    const handleBlur = () => {
        setIsFocused(false);
    };

    const handleInputChange = (event) => {
        setSearchValue(event.target.value);
    };

    const handleSearchClick = () => {
        handleOnSearch(searchValue);
    };

    return (
        <div
            className={`flex items-center justify-center relative transition-all duration-300 ${isFocused ? "w-60" : "w-30"}`}
        >
            <input
                type="text"
                placeholder="Search..."
                value={searchValue}
                onChange={handleInputChange}
                className="w-full p-2 border border-gray-300 rounded-sm focus:outline-none focus:ring-1 bg-white text-black focus:ring-blue-500 pr-10"
                onKeyPress={handleKeyPress}
                onFocus={handleFocus}
                onBlur={handleBlur}
            />
            <span
                className="absolute right-2 text-gray-500 cursor-pointer"
                onClick={handleSearchClick}
            >
                &#128269;
            </span>
        </div>
    );
};

SearchBox.propTypes = {
    handleOnSearch: propTypes.func.isRequired,
};

export default SearchBox;
