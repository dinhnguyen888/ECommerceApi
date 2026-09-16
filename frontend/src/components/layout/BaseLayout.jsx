import PropTypes from "prop-types";
import Header from "../sections/Header";
import Footer from "../sections/Footer";
import SlidingMenu from "../sections/SlidingMenu";
import { MenuProvider } from "../../contexts/MenuContext";

function BaseLayout({ children }) {
    return (
        <MenuProvider>
            <main className="w-full flex flex-col">
                <Header />
                <div className="flex-grow container mx-auto md:px-2 lg:px-28">
                    <div className="w-full bg-white shadow-lg min-h-screen">
                        {children}
                    </div>
                </div>
                <Footer />
                <SlidingMenu />
            </main>
        </MenuProvider>
    );
}

BaseLayout.propTypes = {
    children: PropTypes.node,
};

export default BaseLayout;
