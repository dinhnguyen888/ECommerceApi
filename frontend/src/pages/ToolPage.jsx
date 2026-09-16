import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";
import RelatedContent from "../components/contents/RelatedContent";
import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";

function ToolPage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                Content={<RelatedContent tag={"tool"} />}
                Sidebar={<Filter />}
                Title={
                    <MainTitle
                        text="Tool & Extension"
                        className={"font-semibold uppercase p-8 "}
                    />
                }
            />
        </BaseLayout>
    );
}

export default ToolPage;
