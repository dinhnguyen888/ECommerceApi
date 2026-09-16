import PropTypes from "prop-types";

function Body({ Content, Sidebar, Title }) {
    return (
        <div className="flex flex-col md:flex-row mx-4  ">
            <div className="w-full md:w-4/5 my-5 ">
                {Title}
                <div className="mt-5"> {Content}</div>
            </div>
            <div className="w-full md:w-1/5 md:mt-7">{Sidebar}</div>
        </div>
    );
}

Body.propTypes = {
    Content: PropTypes.node,
    Sidebar: PropTypes.node,
    Title: PropTypes.node,
};

export default Body;
