import SearchBox from "../common/SearchBox";
import Image from "../common/Image";
import { useNavigate } from "react-router-dom";

function Header() {
    const navigate = useNavigate();

    const handleSearch = (keyword) => {
        if (keyword.trim()) {
            navigate(`/search?keyword=${encodeURIComponent(keyword.trim())}`);
        }
    };

    return (
        <header className="relative bg-night-owl shadow-lg py-1 w-full">
            <div className="max-w-screen-xl mx-auto px-4 sm:px-6 lg:px-28">
                <div className="flex justify-between items-center">
                    <Image
                        src="/hutao-avatar.gif"
                        alt="Logo"
                        width={40}
                        height={40}
                        onClickImage={() => (window.location.href = "/")}
                    />
                    <div className="flex flex-row items-center">
                        <SearchBox handleOnSearch={handleSearch} />
                    </div>
                </div>
            </div>
        </header>
    );
}

export default Header;
