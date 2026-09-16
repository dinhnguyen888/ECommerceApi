import { useState } from "react";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";
import MiniNavBar from "../components/common/MiniNavBar";
import BaseLayout from "../components/layout/BaseLayout";
import Body from "../components/sections/Body";
import MiddleBar from "../components/sections/MiddleBar";
import WrappedBody from "../components/sections/WrappedBody";

const sampleFilters = [
    { label: "Đồ án mới nhất", link: "#category1" },
    { label: "Tool mới nhất", link: "#category2" },
    { label: "Đồ án ASP.Net", link: "#category3" },
    { label: "Đồ án ASP.Net Core", link: "#category4" },
    { label: "Acc ref Facebook", link: "#category5" },
    { label: "kèo Airdrop", link: "#category6" },
    { label: "key window bản quyền", link: "#category77" },
    { label: "Iso ghost window 11 ", link: "#category8" },
];

const mininavbarLink = [
    { label: "Sản phẩm", key: "ProductPage" },
    { label: "Bài viết", key: "ArticlePage" },
    { label: "Tin tức", key: "NewsPage" },
];

const Content = () => {
    const [activeBar, setActiveBar] = useState(mininavbarLink[0].key);
    const handleMiniBarClick = (key) => {
        setActiveBar(key);
    };

    return (
        <div className="w-full container ">
            <MiniNavBar
                items={mininavbarLink}
                onClickActiveBar={handleMiniBarClick}
            />

            <WrappedBody
                caseKey={activeBar.toString()}
                caseProp={mininavbarLink}
            />
        </div>
    );
};

function HomePage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={<Content />}
                Sidebar={<Filter filters={sampleFilters} />}
                Title={<MainTitle text="Trang Chủ" className={"font-light"} />}
            />
        </BaseLayout>
    );
}

export default HomePage;
