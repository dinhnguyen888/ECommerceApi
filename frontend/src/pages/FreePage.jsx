import BaseLayout from "../components/layout/BaseLayout";

import MiddleBar from "../components/sections/MiddleBar";

import Body from "../components/sections/Body";
import Filter from "../components/common/Filter";
import MainTitle from "../components/common/MainTitle";

const FreeContent = () => {
    return (
        <>
            <p className="text-2xl font-bold my-20 text-center ">
                Có làm thì mới có ăn :))))))
            </p>
            <span className="text-sm font-light p-4">
                Giỡn thôi đừng giận bạn nhé hihi
            </span>
        </>
    );
};
function FreePage() {
    return (
        <BaseLayout>
            <MiddleBar />
            <Body
                // Content={<RelatedContent tag={"webapp"} />}
                Content={<FreeContent />}
                Sidebar={<Filter />}
            />
        </BaseLayout>
    );
}

export default FreePage;
