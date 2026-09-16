import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";

import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";
import ArticleContent from "../components/contents/ArticleContent";

function ArticlePage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={<ArticleContent />}
                Sidebar={<Filter />}
                Title={
                    <MainTitle
                        text="Bài viết"
                        className={"font-semibold uppercase p-8 "}
                    />
                }
            />
        </BaseLayout>
    );
}

export default ArticlePage;
