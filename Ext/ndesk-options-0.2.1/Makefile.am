nobindir=bin
nolibdir=lib/ndesk-options

OPTIONS_SRC = \
	src/NDesk.Options/AssemblyInfo.cs \
	$(srcdir)/src/NDesk.Options/NDesk.Options/Options.cs

OPTIONS_TEST_SRC = \
	$(srcdir)/src/NDesk.Options/Test/NDesk.Options/OptionContextTest.cs \
	$(srcdir)/src/NDesk.Options/Test/NDesk.Options/OptionSetTest.cs \
	$(srcdir)/src/NDesk.Options/Test/NDesk.Options/OptionTest.cs \
	$(srcdir)/src/NDesk.Options/Test/NDesk.Options/Utils.cs

include doc/Makefile.include

EXTRA_DIST = \
	$(DOC_EXTRA_DIST)                                     \
	$(OPTIONS_TEST_SRC)                                   \
	$(srcdir)/src/NDesk.Options/AssemblyInfo.cs.in        \
	$(srcdir)/src/NDesk.Options/NDesk.Options/Options.cs  \
	$(srcdir)/ndesk-options.spec.in

assemblydir=$(prefix)/lib/ndesk-options
assembly_DATA = \
	$(nolibdir)/NDesk.Options.dll \
	$(srcdir)/src/NDesk.Options/NDesk.Options/Options.cs

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = lib/pkgconfig/ndesk-options.pc

CSC = $(MCS) $(CSFLAGS)

.PHONY: all check rpm

all: $(nolibdir)/NDesk.Options.dll

dist-hook:
	if [ -d $(srcdir)/.git ] ; then \
		(cd $(srcdir) && git-log > $(distdir)/ChangeLog) ; \
	fi
	
$(nolibdir)/NDesk.Options.dll: $(OPTIONS_SRC)
	-mkdir -p `dirname $@`
	$(CSC) -debug+ $(OPTIONS_SRC) -langversion:ISO-2 -t:library -out:$@

$(nolibdir)/NDesk.Options_test.dll: $(nolibdir)/NDesk.Options.dll $(OPTIONS_TEST_SRC)
	-mkdir -p `dirname $@`
	$(CSC) -debug+ -t:library -out:$@ -r:$(nolibdir)/NDesk.Options.dll \
		-r:nunit.core.dll -r:nunit.framework.dll -r:nunit.util.dll \
		$(OPTIONS_TEST_SRC) 

$(nolibdir)/NDesk.Options_test_linq.dll: $(OPTIONS_SRC) $(OPTIONS_TEST_SRC)
	-mkdir -p `dirname $@`
	$(CSC) -debug+ -d:LINQ -t:library -out:$@ \
		-r:nunit.core.dll -r:nunit.framework.dll -r:nunit.util.dll \
		$(OPTIONS_SRC) $(OPTIONS_TEST_SRC) 

check: $(nolibdir)/NDesk.Options_test.dll $(nolibdir)/NDesk.Options_test_linq.dll
	nunit-console2 $(nolibdir)/NDesk.Options_test.dll
	nunit-console2 $(nolibdir)/NDesk.Options_test_linq.dll

rpm:
	tgz=$(DIST_ARCHIVES); \
	if [ ! -e $(DIST_ARCHIVES) ] ; then tgz=archive/$(DIST_ARCHIVES) ; fi ; \
	cp $$tgz /usr/src/packages/SOURCES/$(DIST_ARCHIVES) && \
		rpmbuild -bb ndesk-options.spec && \
		cp /usr/src/packages/RPMS/noarch/$(distdir)*.rpm archive

bin-zip:
	-rm archive/$(distdir).bin.zip
	$(MAKE) prefix= DESTDIR=$(distdir).bin install && \
		zip -r archive/$(distdir).bin.zip $(distdir).bin && \
		rm -Rf $(distdir).bin

prepare-release:
	$(MAKE) distcheck
	$(MAKE) bin-zip
	$(MAKE) doc-html
	mv $(DIST_ARCHIVES) archive

CLEANFILES = \
	$(DOC_CLEANFILES) \
	$(nolibdir)/NDesk.Options.dll \
	$(nolibdir)/NDesk.Options.dll.mdb \
	$(nolibdir)/NDesk.Options_test.dll \
	$(nolibdir)/NDesk.Options_test.dll.mdb \
	$(nolibdir)/NDesk.Options_test_linq.dll \
	$(nolibdir)/NDesk.Options_test_linq.dll.mdb \
	$(nolibdir)/TestResult.xml

