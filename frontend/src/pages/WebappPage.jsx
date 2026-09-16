import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";
import RelatedContent from "../components/contents/RelatedContent";
import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";

function WebappPage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={<RelatedContent tag={"webapp"} />}
                Sidebar={<Filter />}
                Title={
                    <MainTitle
                        text="Web vs App"
                        className={"font-semibold uppercase p-8 "}
                    />
                }
            />
        </BaseLayout>
    );
}

export default WebappPage;
