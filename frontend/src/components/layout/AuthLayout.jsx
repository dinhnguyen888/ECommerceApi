import PropTypes from "prop-types";
import Header from "../sections/Header";
import Footer from "../sections/Footer";

function AuthLayout({ children }) {
    return (
        <main className="h-screen w-full shadow-lg ">
            <Header />
            <div className="mt-11">
                <div className="container mx-auto md:px-6 lg:px-28 py-12">
                    <div className="max-w-md mx-auto border bg-white shadow-lg rounded-md p-8">
                        {children}
                    </div>
                </div>
            </div>
            <Footer />
        </main>
    );
}

AuthLayout.propTypes = {
    children: PropTypes.node,
};

export default AuthLayout;
