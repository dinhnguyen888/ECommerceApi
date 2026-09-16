import { createContext, useState, useContext } from "react";
import PropTypes from "prop-types";

const MenuContext = createContext();

export const MenuProvider = ({ children }) => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const toggleMenu = () => {
        setIsMenuOpen((prev) => !prev);
    };

    return (
        <MenuContext.Provider value={{ isMenuOpen, toggleMenu }}>
            {children}
        </MenuContext.Provider>
    );
};
MenuProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export const useMenu = () => {
    return useContext(MenuContext);
};
